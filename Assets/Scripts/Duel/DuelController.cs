using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelController : MonoBehaviour
{   

    [SerializeField] Inventory playerInventory, enemyInventory;
    Card playerMove, enemyMove;

    public Card PlayerMove {get => playerMove; set => playerMove = value;}
    public Card EnemyMove {get => enemyMove; set => enemyMove = value;}

    public bool enemyChoiceVisible = false;

    /* game model variables and objects */

    public enum DuelStatus
    {
        Continue,
        PlayerWin,
        EnemyWin,
        Draw
    }

    [System.Serializable] // do I need this
    public enum DuelID
    {
        KeyboardWarrior,
        Stalker,
        ComputerBug
    }
    [SerializeField] DuelID duelID;

    DuelPlayer player, enemy;
    public DuelPlayer Player {get => player;}
    public DuelPlayer Enemy {get => enemy;}

    int round = 1;
    int turn = 0; 
    int dmg; // holds last damage
    List<Special> activeSpecials = new List<Special>();
    List<Card> playerActionHistory = new List<Card>();
    List<Card> enemyActionHistory = new List<Card>();

    public int RoundNo()
    {
        return round;
    }

    public int TurnNo()
    {
        return turn;
    }

    /* game state helper methods */

    public void initDuel()
    {
        switch (duelID) {
            case DuelID.KeyboardWarrior:
                break;
            case DuelID.Stalker:
                break;
            case DuelID.ComputerBug:
                break;
            default:
                Debug.LogError(
                        "Invalid DuelID - is this set in the inspector? ID: " +
                        duelID.ToString()
                        );
                break;
        }
        enemyChoiceVisible = false; // stop seeing for now

        // no specials yet to trigger

        // TODO: Unclear if this is needed or if this is handled by the UI
        // Player attacks on the first turn (0), defends on the second turn (1)
        // so needs to draw a defense card on the first turn (0)
        // and an attack card on the second turn (1)
        string playerCardType = (turn == 0) ? "Defense" : "Attack";
        bool success = player.DrawCard(playerCardType);
        // Enemy defends on first turn (0), attacks on second turn (1)
        // so needs to draw an attack card at top of first turn (0)
        // and a defense card on the second turn (1)
        string enemyCardType = (turn == 1) ? "Attack" : "Defense";
        success = enemy.DrawCard(enemyCardType);
        enemy.PlayCard(
                enemyCardType,
                cardIndex(enemyMove, enemy, enemyCardType)
                );
        // this is needed in order to see
        enemyMove = cpuChooseCard(enemyCardType);

        // do we need to do this too?
        enemy.PlayCard(
                enemyCardType,
                cardIndex(enemyMove, enemy, enemyCardType)
                );

    }

    public DuelStatus status()
    {
        if (Player.HP == 200) return DuelStatus.PlayerWin;
        if (Player.HP == 0) return DuelStatus.EnemyWin;
        if (round >= 10) return DuelStatus.Draw;
        return DuelStatus.Continue;
    }

    private void useSpecial(Special s)
    /* TODO: This will contain the implementation/calls to the implementation
     * for all the specials:
       Barbell Medic
       Full Thrust
       Parry Reposte
       FAHVES
       Ulam Equilibrium
       M?ng Tom?s
       Chocododge
       How Can You See??
       Chinese Red Vest
       SORD
       NO
       */
    {
        // run now
        switch (s.name) {
            case "Barbell Medic":
                break;
            case "Full Thrust":
                break;
            case "Parry Reposte":
                break;
            case "FAHVES":
                break;
            case "Ulam Equilibrium":
                break;
            case "M?ng Tom?s":
                break;
            case "Chocododge":
                break;
            case "How Can You See??":
                enemyChoiceVisible = true; // that's it
                break;
            case "Chinese Red Vest":
                break;
            case "SORD":
                break;
            case "NO":
                break;
            default:
                Debug.LogError("Invalid Special name: "+ s.name);
                break;
        }
    }

    private void runSpecials(bool afterDamage = false)
    /* applies effects in the specials queue, decrements duration of each one
     * that runs
     * */
    {
        // timing 1 means it runs when afterDamage = true
        int timing = (afterDamage) ? 1 : 0;
        foreach (Special s in activeSpecials) {
            if (s.timing == timing) {
                useSpecial(s);
            }
            s.duration--;
            if (s.duration <= 1) {
                bool success = activeSpecials.Remove(s);
                if (!success) {
                    Debug.LogError(
                            "Couldn't remove special from the queue: " +
                            s.name +
                            " duration: " +
                            s.duration
                            );
                }
            }
        }
    }

    /* CPU action selection */

    private int simulateAttack(Card aCard, Card dCard, DuelPlayer attacker, DuelPlayer defender)
    {
        Attack attack = parseAttackString(aCard.Type);
        Defense defense = parseDefenseString(dCard.Type);
        DuelWeapon weapon = attacker.Weapon;
        DuelArmor armor = defender.Armor;

        return Damage.damage(
                attack,
                defense,
                aCard.Strength,
                dCard.Strength,
                weapon.weaponType,
                weapon.rating,
                armor.armorType,
                armor.rating
                );
    }

    const int _CARD_NOT_FOUND = -1;

    private int cardIndex(Card c, DuelPlayer p, string cardType)
    {
        List<Card> h;
        if (cardType == "Attack") {
            h = p.AttackHand;
        } else {
            h = p.DefenseHand;
        }
        for (int i = 0; i < h.Count; i++) {
            if (c.Type == h[i].Type && c.Strength == h[i].Strength) {
                return i;
            }
        }
        Debug.LogError("Could not find card with " +
                c.AllCardInfo() + " " +
                "in player " + p.Name + " " + cardType + " hand");
        return _CARD_NOT_FOUND;
    }

    private Card cpuMaxNeutralAttack()
    /*
     * Gets the best attack that causes the most damage against a corresponding
     * neutral defense with strength 5.
     */
    {
        int best = 0;
        int i = 0;
        int best_i = 0;
        foreach (Card aCard in enemy.AttackHand) {
            Defense d = Damage.neutralDefenseForAttack(parseAttackString(aCard.Type));
            Card dCard =Card.MakeDefenseCard(5, d.ToString());
            int result = simulateAttack(aCard,dCard,enemy,player);
            if (result > best) {
                best = result;
                best_i = i;
            }
            i++;
        }
        return enemy.AttackHand[best_i];
    }

    private Card cpuMinNeutralDefense()
    /*
     * Gets the best defense that receives the least damage against a corresponding
     * neutral attack with strength 5.
     */
    {
        int best = 200;
        int i = 0;
        int best_i = 0;
        foreach (Card dCard in enemy.DefenseHand) {
            Attack a = Damage.neutralAttackForDefense(parseDefenseString(dCard.Type));
            Card aCard = Card.MakeAttackCard(5,a.ToString());
            int result = simulateAttack(aCard,dCard,player,enemy);
            if (result < best) {
                best = result;
                best_i = i;
            }
            i++;
        }
        return enemy.DefenseHand[best_i];
    }

    private Card cpuMaxEffectiveAttack()
    /*
     * Gets the attack that causes the most damage against a corresponding
     * defense defense with strength 5.
     */
    {
        int best = 0;
        int i = 0;
        int best_i = 0;
        foreach (Card aCard in enemy.AttackHand) {
            Defense d = Damage.weakestDefenseForAttack(parseAttackString(aCard.Type));
            Card dCard = Card.MakeDefenseCard(5, d.ToString());
            int result = simulateAttack(aCard,dCard,enemy,player);
            if (result > best) {
                best = result;
                best_i = i;
            }
            i++;
        }
        return enemy.AttackHand[best_i];
    }
    
    private Card cpuMinWeakDefense()
    /*
     * Gets the best defense that receives the least damage against a corresponding
     * neutral attack with strength 5.
     */
    {
        int best = 200;
        int i = 0;
        int best_i = 0;
        foreach (Card dCard in enemy.DefenseHand) {
            Attack a = Damage.effectiveAttackForDefense(parseDefenseString(dCard.Type));
            Card aCard = Card.MakeAttackCard(5,a.ToString());
            int result = simulateAttack(aCard,dCard,player,enemy);
            if (result < best) {
                best = result;
                best_i = i;
            }
            i++;
        }
        return enemy.DefenseHand[best_i];
    }

    private Card cpuImitateAttack()
    {
        if (round > 1) {
            // round 1 at index 0, round 2 at index 2, etc.
            int i = (round - 2) * 2; 
            Card lastPlayerAttack = playerActionHistory[i];
            string aStr = lastPlayerAttack.Type;
            foreach (Card c in enemy.AttackHand) {
                if (c.Type == aStr) {
                    return c;
                }
            }
        }
        return enemy.AttackHand[0];
    }

    private Card cpuImitateDefense()
    {
        if (round > 1) {
            // round 1 at index 0, round 2 at index 2, etc.
            int i = (round - 2) * 2; 
            Card lastPlayerDefense = playerActionHistory[i];
            string dStr = lastPlayerDefense.Type;
            foreach (Card c in enemy.DefenseHand) {
                if (c.Type == dStr) {
                    return c;
                }
            }
        }
        return enemy.DefenseHand[0];
    }

    private Card cpuChooseCard(string cardType)
    {

        if (cardType == "Attack") {
            // select attack
            switch (duelID) {
                case DuelID.KeyboardWarrior:
                    return cpuMaxNeutralAttack();
                case DuelID.Stalker:
                    return cpuImitateAttack();
                case DuelID.ComputerBug:
                    return cpuMaxEffectiveAttack();
                default:
                    Debug.LogError(
                            "Invalid DuelID - is this set in the inspector? ID: " +
                            duelID.ToString()
                            );
                    return cpuMaxNeutralAttack();
            }
        } else {
            // select defense
            switch (duelID) {
                case DuelID.KeyboardWarrior:
                    return cpuMinNeutralDefense();
                case DuelID.Stalker:
                    return cpuImitateDefense();
                case DuelID.ComputerBug:
                    return cpuMinWeakDefense();
                default:
                    Debug.LogError(
                            "Invalid DuelID - is this set in the inspector? ID: " +
                            duelID.ToString()
                            );
                    return cpuMaxNeutralAttack();
            }
         }
    }

    private Attack parseAttackString(string cs)
    {
       switch (cs) {
           case "S" :
           case "Strike" :
               return Attack.Strike;
           case "L" :
           case "Lash" :
               return Attack.Lash;
           case "T" :
           case "Thrust" :
               return Attack.Thrust;
           default : {
                         Debug.LogError("Can't parse attack string: " + cs);
                         return Attack.Strike;
                     }
       }
    }

    private Defense parseDefenseString(string cs)
    {
        switch (cs) {
            case "D" :
            case "Dodge" :
                return Defense.Dodge;
            case "B" :
            case "Block" :
                return Defense.Block;
            case "P" :
            case "Parry" :
                return Defense.Parry;
            default : {
                          Debug.LogError("Can't parse attack string: " + cs);
                          return Defense.Dodge;
                      }
        }
    }

    private Card getAttackCard()
    {
        if (playerMove.CardClass == "Attack") {
            return playerMove;
        }
        return enemyMove;
    }

    private Card getDefenseCard()
    {
        if (playerMove.CardClass == "Defense") {
            return playerMove;
        }
        return enemyMove;
    }

    private DuelWeapon getAttackWeapon()
    {
        if (playerMove.CardClass == "Attack") {
            return player.Weapon;
        }
        return enemy.Weapon;
    }

    private DuelArmor getDefenseArmor()
    {
        if (playerMove.CardClass == "Defense") {
            return player.Armor;
        }
        return enemy.Armor;
    }

    private int resolveDamage()
    {
        Card aCard = getAttackCard(); 
        Card dCard = getDefenseCard();
        Attack attack = parseAttackString(aCard.Type);
        Defense defense = parseDefenseString(dCard.Type);
        DuelWeapon weapon = getAttackWeapon();
        DuelArmor armor = getDefenseArmor();

        int result = Damage.damage(
                attack,
                defense,
                aCard.Strength,
                dCard.Strength,
                weapon.weaponType,
                weapon.rating,
                armor.armorType,
                armor.rating
                );
        if (turn == 0) {
            player.HP += result; // player is attacking
            enemy.HP -= result; // enemy is defending
        } else {
            player.HP -= result; // player is defending
            enemy.HP += result; // player is attacking
        }
        return result;
    }

    private void buildHistory()
    {
       playerActionHistory.Add(playerMove);
       enemyActionHistory.Add(enemyMove);
    }

    private void nextTurn()
    {
        buildHistory();
        if (turn == 1) {
            turn = 0;
            round++; // next round
        }
    }

    /* specials code */

    /* Controller Lifecycle and Triggered Actions */

    private void Awake() {
        player = new DuelPlayer("Pkmn Trainer Red", 
            new DuelWeapon(playerInventory.EquipedWeapon), 
            new DuelArmor(playerInventory.EquipedArmor),
            Equipment.InventoryToEquipment(playerInventory));
        
        enemy = new DuelPlayer("Pkmn Trainer Blue", 
            new DuelWeapon(enemyInventory.EquipedWeapon), 
            new DuelArmor(enemyInventory.EquipedArmor),
            Equipment.InventoryToEquipment(enemyInventory));
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    void playerChoosesSpecial(Special s)
    /*
     * Player chooses a special.
     */
    {
        // trigger effects if duration is zero
        if (s.duration == 0 && s.timing == 0) {
            useSpecial(s);
        }
        // else add to specials list
        activeSpecials.Add(s);
    }

    void playerChoosesCard(Card c)
    {
        // is player attacking or defending?
        // set player move
        playerMove = c;
        // enemyMove is already chosen
        // resolve damage
        dmg = resolveDamage(); // resolve damage doesn't need any parameters

        // TODO: this is where the update damage UI would happen
        // Here's additional information that might be useful for the UI
        // attack type and weapon
        Card attackC = getAttackCard();
        DuelWeapon attackW = getAttackWeapon();
        Attack attack = parseAttackString(attackC.Type);
        // defense type and armor
        Card defenseC = getDefenseCard();
        DuelArmor defenseA = getDefenseArmor();
        Defense defense = parseDefenseString(defenseC.Type);
        // Some nice info for custom messages
        bool hit = ! Damage.isCounter(attack,defense);
        bool criticalHit = Damage.isEffective(attack,defense);
        bool weaponBonus = hit && Damage.isWeaponSpecialty(attack, attackW.weaponType);
        bool armorBonus = hit && Damage.isArmorSpecialty(defense, defenseA.armorType);
        // TODO: UI update calls here?

        // trigger post damage resolution specials
        runSpecials(afterDamage: true);
        // check duel status
        if (status() == DuelStatus.Continue) {
            // next turn
            nextTurn();
            enemyChoiceVisible = false; // stop seeing for now

            // trigger pre attack specials in queue
            runSpecials(afterDamage: false);

            // TODO: Unclear if this is needed or if this is handled by the UI
            // Player attacks on the first turn (0), defends on the second turn (1)
            // so needs to draw a defense card on the first turn (0)
            // and an attack card on the second turn (1)
            string playerCardType = (turn == 0) ? "Defense" : "Attack";
            bool success = player.DrawCard(playerCardType);
            // Enemy defends on first turn (0), attacks on second turn (1)
            // so needs to draw an attack card at top of first turn (0)
            // and a defense card on the second turn (1)
            string enemyCardType = (turn == 1) ? "Attack" : "Defense";
            success = enemy.DrawCard(enemyCardType);

            // this is needed in order to see
            enemyMove = cpuChooseCard(enemyCardType);

            // do we need to do this too?
            enemy.PlayCard(
                    enemyCardType,
                    cardIndex(enemyMove, enemy, enemyCardType)
                    );

        } else if (status() == DuelStatus.PlayerWin) {
            // run any special conditions for player winning
        } else if (status() == DuelStatus.EnemyWin) {
            // run any special conditions for player losing
        }
    }

    public void FakeTurn(Card played)
    {
        playerMove = played;
        // Is the enemy Attacking or Defending? 
        string enemyHand = (played.CardClass == "Attack")? "Defense": "Attack";
        
        // Enemy plays a random card from their hand
        Card enemyPlayed = enemy.PlayCard(enemyHand, Random.Range(0, 6));
        enemyMove = enemyPlayed;

        enemy.DrawCard(enemyHand);
        Debug.Log($"<color=orange><color=red>{player.Name}</color> Played {played.Name} <color=aqua>{enemy.Name}</color> Played {enemyPlayed.Name} </color>");
        
        int changeToHP = 0;
        
        string attacker = (played.CardClass == "Attack")? played.Type : enemyPlayed.Type;
        string defender = (played.CardClass == "Defense")? played.Type : enemyPlayed.Type;
        
        switch(attacker, defender)
        {
            case ("S", "P"):
            case ("T", "B"):
            case ("L", "D"):
                // CRITICAL DAMAGE BRO
                Debug.Log("<color=yellow>CRITICAL HIT</color>");
                changeToHP = Mathf.Abs(played.Strength - enemyPlayed.Strength) * 5;
                break;
            case ("S", "D"):
            case ("T", "P"):
            case ("L", "B"):
                // Counter - no change to HP
                Debug.Log("<color=yellow>FULL COUNTER</color>");
                break;
            default:
                // PHHHHSSSSSS casual
                Debug.Log("<color=yellow>CONTACT!</color>");
                changeToHP = Mathf.Abs(played.Strength - enemyPlayed.Strength)*2;
                break;
        }

        // Update the player HP. UI Only looks at Player HP and computes enemy value
        Debug.Log($"<color=orange>{player.Name}'s stress has changed by </color><color=green><b>{changeToHP}</b></color>");
        player.HP += (played.CardClass == "Attack")? changeToHP : -1 * changeToHP;
    }
}
