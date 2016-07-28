using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextController : MonoBehaviour
{
    #region Instance Variables
    public Text text;

    private static int MaxPistolAmmo = 9;
    private static int MaxRPGAmmo = 5;
    private static int MaxDukeHealth = 12;
    private static int MaxLizardHealth = 2;

    //states that things, including duke, can be in
    private enum States
    {
        start, //state for duke only
        rooftop,
        street_below_rooftop,
        dilapidated_apartment,
        theater_front,
        apartment_room,
        theater_inside,
        finished, //state for duke only
        dead, //state for duke only
        on_duke, //state for getable items only
        doesnt_exist //state for things that will spawn later or are destroyed
    };

    //getable item states
    private States PistolLocation = States.rooftop;
    private States RPGLocation = States.dilapidated_apartment;
    private States PistolAmmoLocation = States.dilapidated_apartment;
    private States RPGAmmoLocation = States.theater_inside;
    private int PistolAmmoAmount = MaxPistolAmmo;
    private int RPGAmmoAmount = MaxRPGAmmo;

    //enemy states
    private States Lizard1Location = States.street_below_rooftop;
    private States Lizard2Location = States.theater_front;
    private int Lizard1Health = MaxLizardHealth;
    private int Lizard2Health = MaxLizardHealth;

    //Duke's states
    private States DukeLocation = States.start;
    private int DukeHealth = MaxDukeHealth;

    //Other States
    private States CannisterLocation = States.rooftop;
    private States DuctLocation = States.doesnt_exist;
    private States LizWindowLocation = States.doesnt_exist;
    private States BoardedDoorLocation = States.theater_front;
    private States BlastedDoorLocation = States.doesnt_exist;

    private int won = 0;

    private string AdditionalText = "";

    #endregion

    void Tick()
    {
        AdditionalText = "";
        if (Lizard1Location == DukeLocation)
        {
            DukeHealth -= Lizard1Health;
        }else if (Lizard2Location == DukeLocation)
        {
            DukeHealth -= Lizard2Health;
        }
    }

    void HandlePistolState()
    {
        if (PistolLocation == DukeLocation)
        {
            if (PistolAmmoAmount == MaxPistolAmmo)
            {
                text.text += "\n\nSomebody threw away a perfectly good, fully-loaded Desert Eagle .357 Magnum pistol.  " +
                    "<color=lime>Press T to take pistol.</color>\n";
            }
            else
            {
                text.text += "\n\nSomebody threw away a perfectly good Desert Eagle .357 Magnum pistol.  " +
                    "<color=lime>Press T to take pistol.</color>\n";
            }
            if (Input.GetKeyDown("t"))
            {
                PistolLocation = States.on_duke;
                Tick();
            }
        }
        else if (PistolLocation == States.on_duke)
        {
            text.text += "\n\nDuke is packing heat!  <color=lime>Press D to drop pistol.  Press F to fire pistol.</color>\n";
            if (Input.GetKeyDown("d"))
            {
                PistolLocation = DukeLocation;
                Tick();
            }
            else if (Input.GetKeyDown("f"))
            {
                Tick();
                if (PistolAmmoAmount > 0)
                {
                    AdditionalText = "";
                    for (int i = 0; i < PistolAmmoAmount; i++)
                    {
                        AdditionalText += " ";
                    }
                    AdditionalText += "Bang!";
                    PistolAmmoAmount--;
                    if (CannisterLocation == DukeLocation)
                    {
                        CannisterLocation = States.doesnt_exist;
                        DuctLocation = DukeLocation;
                        AdditionalText += "\n\nThe \"imflammable\" canisters exploded!  Who would have thought " +
                            "that \"imflammable\" means \"flammable\"!\n";
                    }else if(Lizard1Location == DukeLocation)
                    {
                        if(Lizard1Health == 0)
                        {
                            AdditionalText += "\n\nGibs fly.";
                        }else
                        {
                            AdditionalText += "\n\nThe lizard man growls in pain from being shot!\n";
                            Lizard1Health -= 1;
                            if (Lizard1Health == 0)
                            {
                                AdditionalText += "\n\nThe lizard man dies!\n";
                                LizWindowLocation = Lizard1Location;
                            }
                        }
                    }
                    else if (Lizard2Location == DukeLocation)
                    {
                        if (Lizard2Health == 0)
                        {
                            AdditionalText += "\n\nGibs fly.\n";
                        }
                        else
                        {
                            AdditionalText += "\n\nThe lizard man growls in pain from being shot!\n";
                            Lizard2Health -= 1;
                            if (Lizard2Health == 0)
                            {
                                AdditionalText += "\n\nThe lizard man dies!\n";
                            }
                        }
                    }
                }
                else
                {
                    AdditionalText = "(click)";
                }
            }
        }
    }


    void HandleRPGState()
    {
        if (RPGLocation == DukeLocation)
        {
           
            text.text += "\n\nThere is a Rocket Propelled Grenade launcher--someone has some heavy artillery!  " +
                    "<color=lime>Press G to take RGP.</color>\n";
            
            if (Input.GetKeyDown("g"))
            {
                RPGLocation = States.on_duke;
                Tick();
            }
        }
        else if (RPGLocation == States.on_duke)
        {
            text.text += "\n\nDuke is packing Serious heat!  <color=lime>Press E to drop RPG.  Press B to fire RPG.</color>\n";
            if (Input.GetKeyDown("e"))
            {
                RPGLocation = DukeLocation;
                Tick();
            }
            else if (Input.GetKeyDown("b"))
            {
                Tick();
                if (RPGAmmoAmount > 0)
                {
                    AdditionalText = "";
                    for (int i = 0; i < RPGAmmoAmount; i++)
                    {
                        AdditionalText += " ";
                    }
                    AdditionalText += "***HUGE EXPLOSION!***";
                    RPGAmmoAmount--;
                    if(DukeLocation == States.apartment_room || DukeLocation == States.dilapidated_apartment 
                        || DukeLocation == States.theater_inside)
                    {
                        DukeHealth -= 4;
                        AdditionalText += "\n\nThe RPG explosion inside such a small space hurts Duke badly!\n";
                    }
                    else if (Lizard1Location == DukeLocation)
                    {
                        if (Lizard1Health == 0)
                        {
                            AdditionalText += "\n\nThe remains of the lizard man vaporize in a ball of fire!  Not a trace is left.\n";
                            Lizard1Location = States.doesnt_exist;
                        }
                        else
                        {
                            AdditionalText += "\n\nThe lizard man is blasted into gibs!\n";
                            Lizard1Health =0;
                            LizWindowLocation = Lizard1Location;
                        }
                    }
                    else if (Lizard2Location == DukeLocation)
                    {
                        if (Lizard2Health == 0)
                        {
                            AdditionalText += "\n\nThe remains of the lizard man vaporize in a ball of fire!  Not a trace is left.\n";
                            Lizard2Location = States.doesnt_exist;
                        }
                        else
                        {
                            AdditionalText += "\n\nThe lizard man is blasted into gibs!\n";
                            Lizard2Health = 0;
                        }
                    }else if (BoardedDoorLocation == DukeLocation)
                    {
                        AdditionalText = "\n\nThe door is blasted, leaving a gaping hole!";
                        BoardedDoorLocation = States.doesnt_exist;
                        BlastedDoorLocation = DukeLocation;
                    }
                }
                else
                {
                    AdditionalText = "A computery voice says, \"I am sorry but there is no ammunition remaining.  " + 
                        "To order new ammunition, call 876-5309 and ask for Jenny.  It takes 6 to 8 weeks" + 
                        "for ammuntion to be delivered.  You must be eighteen or older to buy ammunition from Jenny's " + 
                        "Weapon Supply.\"\n";
                }
            }
        }
    }

    void HandleLizWindowState()
    {
        if(LizWindowLocation == DukeLocation){
            text.text += "\n\nThere is an open window in the side of a nearby building.  " +
                "<color=lime>Press W to crawl through the window.</color>\n";
            if (Input.GetKeyDown("w"))
            {
                Tick();
                AdditionalText = "Duke crawls through the window.\n";
                DukeLocation = States.dilapidated_apartment;
            }

        }
    }

    void HandleCannisterState()
    {
        if (CannisterLocation == DukeLocation)
        {
            text.text += "\n\nFor some reason, there is a cluster of yellow gas canisters marked \"Imflammable\" " +
                "against the ledge obscuring what appears to be a vent.  <color=lime>Press S to study canisters.</color>\n";

            if (Input.GetKeyDown("s"))
            {
                Tick();
                AdditionalText = "Duke Nukem muses... \"It's " + 
                    "a good thing they are imflammable instead of flammable, or we could have an explosion!\"\n";
            }
        }
    }

    void HandleDuctState()
    {
        if (DuctLocation == DukeLocation)
        {
            text.text += "\n\nThe explosion blew a gaping hole in the air conditioning duct " +
                "that was hidden behind the canisters.  " +
                "<color=lime>Press J to jump into the duct.</color>\n";

            if (Input.GetKeyDown("j"))
            {
                Tick();
                AdditionalText = "Duke takes a long fall through a long duct, " +
                    "but miraculously lands on his feet and is only slightly dazed.\n";
                DukeLocation = States.street_below_rooftop;
            }
        }
    }

    void HandleBoardedDoorState()
    {
        if(BoardedDoorLocation == DukeLocation)
        {
            text.text += "\n\nThe entrance is boarded up.\n";
        }

    }

    void HandleBlastedDoorState()
    {
        if (BlastedDoorLocation == DukeLocation)
        {
            text.text += "\n\nThe entrance has a large hole blasted in it.  " + 
                "<color=lime>Press H to crawl through the hole.</color>\n";
            if (Input.GetKeyDown("h"))
            {
                Tick();
                AdditionalText = "Duke crawls through the hole.\n";
                DukeLocation = States.theater_inside;
            }

        }

    }

    void HandleLizard1State()
    {
        if(Lizard1Location == DukeLocation)
        {
            switch (Lizard1Health)
            {
                case 0:
                    text.text += "\n\nThere is a dead lizard man below an open window.\n";
                    break;
                case 1:
                    text.text += "\n\nThere is a very angry wounded lizard man shooting at you!\n";
                    break;
                case 2:
                    text.text += "\n\nThere is a growling lizard man shooting at you!\n";
                    break;
                default:
                    break;
            }
        }
    }

    void HandleLizard2State()
    {
        if (Lizard2Location == DukeLocation)
        {
            switch (Lizard2Health)
            {
                case 0:
                    text.text += "\n\nThere is a dead lizard man visible inside the ticket window.\n";
                    break;
                case 1:
                    text.text += "\n\nThere is a very angry wounded lizard man inside the " + 
                        "ticket window shooting at you!\n";
                    break;
                case 2:
                    text.text += "\n\nThere is a growling lizard man inside the ticket window shooting at you!\n";
                    break;
                default:
                    break;
            }
        }
    }


    #region Start and Update
    // Use this for initialization
    void Start()
    {
        DukeLocation = States.start;
    }

    // Update is called once per frame
    void Update()
    {
        print(DukeLocation);
        switch (DukeLocation)
        {
            case States.start:
                StartOfGame();
                break;
            case States.rooftop:
                Rooftop();
                break;
            case States.apartment_room:
                ApartmentRoom();
                break;
            case States.dead:
                Dead();
                break;
            case States.dilapidated_apartment:
                DilapidatedApartment();
                break;
            case States.finished:
                Finished();
                break;
            case States.street_below_rooftop:
                StreetBelowRooftop();
                break;
            case States.theater_front:
                TheaterFront();
                break;
            case States.theater_inside:
                TheaterInside();
                break;
            default:
                Stub();
                break;
        }
        text.text = "<color=aqua>" + AdditionalText + "</color>\n\n" + text.text;
        if (DukeHealth > 0 && won==0)
        {
            HandlePistolState();
            HandleCannisterState();
            HandleDuctState();
            HandleLizard1State();
            HandleLizard2State();
            HandleLizWindowState();
            HandleBoardedDoorState();
            HandleBlastedDoorState();
            HandleRPGState();
            print(DukeHealth);
        }
        if(DukeHealth <= 0)
        {
            DukeLocation = States.dead;
        }else if(DukeHealth <= MaxDukeHealth / 2)
        {
            text.text = "<color=red>Duke is in pain!</color>\n" + text.text;
            if(Lizard1Location==DukeLocation && Lizard1Health > 0)
            {
                text.text = "<color=lime>Maybe you should fire back at the lizard man?</color>\n" + text.text;
            }else if (Lizard2Location == DukeLocation && Lizard2Health > 0)
            {
                text.text = "<color=lime>Maybe you should fire back at the lizard man?</color>\n" + text.text;
            }
        }
    }

    #endregion

    #region State Handler Region
    void ApartmentRoom()
    {
        text.text = "<color=yellow>Duke is in the bedroom of somebody's dilapidated apartment.  " +
            "There is a TV with a busted screen in the corner.  There are stains on the ratty carpet. There are pinups on the walls.</color>  " +
            "There is a window to the outside.  Another room is to the right.  " +
            "<color=lime>Press R to enter the room on the right.  Press X to exit through the window.</color>\n";

        if (Input.GetKeyDown("r"))
        {
            Tick();
            DukeLocation = States.dilapidated_apartment;
        }

        if (Input.GetKeyDown("x"))
        {
            Tick();
            DukeLocation = States.theater_front;
        }

    }

    void Dead()
    {
        text.text = "The Duke is Dead.  <color=lime>Press any key to play again.</color>\n";

        if (Input.anyKeyDown)
        {
            DukeLocation = States.start;
            StartOfGame();
        }

    }

    void DilapidatedApartment()
    {
        text.text = "<color=yellow>Duke is in the living room of somebody's dilapidated apartment.  " +
            "There is a ratty mattress on the floor, and in fact, Duke thinks he might have heard " +
            "some squeaking sounds.  There are pinups on the walls.  The carpet is worn to the oily, dingy concrete floor in places</color>" +
            "Another room is to the left.  There is a window to the street.  " +
            "<color=lime>Press L to enter the room on the left.  Press X to exit through the window.</color>\n";

        if (Input.GetKeyDown("l"))
        {
            Tick();
            DukeLocation = States.apartment_room;
        }

        if (Input.GetKeyDown("x"))
        {
            Tick();
            DukeLocation = States.street_below_rooftop;
        }
    }

    void Finished()
    {
        text.text = "Hail to the King, Baby! Duke won the game!  Duke says, \"To be continued....sometime.\"  <color=lime>Press any key to play again.</color>\n";
        won = 1;

        if (Input.anyKeyDown)
        {
            Tick();
            DukeLocation = States.start;
        }

    }

    void StreetBelowRooftop()
    {
        text.text = "<color=yellow>Duke is on the ground next to a moderately-tall building.  </color>" +
            "A trash-strewn street goes to the left.  <color=lime>Press L to walk down the street to the left.</color>\n";

        if (Input.GetKeyDown("l"))
        {
            Tick();
            DukeLocation = States.theater_front;
        }
    }

    void TheaterFront()
    {
        text.text = "<color=yellow>Duke is standing in front of an X-rated theater that in a state of disrepair.  "
            + "There is a small ticket " +
            "window just next to the entrance doors.</color>  There is a large open window into a nearby building.  A trash-strewn street goes to the right.  <color=lime>Press R to walk " +
            "up the street to the right.  Press W to crawl through the large open window.</color>\n";


        if (Input.GetKeyDown("r"))
        {
            Tick();
            DukeLocation = States.street_below_rooftop;
        }

        if (Input.GetKeyDown("w"))
        {
            Tick();
            DukeLocation = States.apartment_room;
        }
    }

    void TheaterInside()
    {
        text.text = "<color=yellow>Duke is standing inside of an X-rated theater that in a state of disrepair.  "
            + "There are booths against the wall and explicit posters on another wall.</color>  " + 
            "There is a door with a gaping hole out to the street.  Press H to crawl through the hole.</color>\n";

        if (Input.GetKeyDown("h"))
        {
            Tick();
            DukeLocation = States.theater_front;
        }

        if(Lizard1Health==0 && Lizard2Health == 0)
        {
            DukeLocation = States.finished;
        }
    }

    void StartOfGame()
    {
        PistolLocation = States.rooftop;
        RPGLocation = States.dilapidated_apartment;
        PistolAmmoLocation = States.dilapidated_apartment;
        RPGAmmoLocation = States.theater_inside;
        PistolAmmoAmount = MaxPistolAmmo;
        RPGAmmoAmount = MaxRPGAmmo;

        //enemy states
        Lizard1Location = States.street_below_rooftop;
        Lizard2Location = States.theater_front;
        Lizard1Health = MaxLizardHealth;
        Lizard2Health = MaxLizardHealth;

        //Duke's states
        DukeLocation = States.start;
        DukeHealth = MaxDukeHealth;

        //Other States
        CannisterLocation = States.rooftop;
        DuctLocation = States.doesnt_exist;
        LizWindowLocation = States.doesnt_exist;
        BoardedDoorLocation = States.theater_front;
        BlastedDoorLocation = States.doesnt_exist;

        won = 0;

        AdditionalText = "";

        text.text = "<color=yellow>Duke Nukem (Duke Nukem and Hollywood Holocaust are trademarks of Gearbox and 3D Realms) is piloting his flying car over a Los Angeles that is " +
                        "burning below him.  He sees a rocket come toward him out of the " +
                        "corner of his eye, and before he can react, hears an explosion and " +
                        "feels the heat of flames.  Gasping for breath, he reaches the Ejector " +
                        "Seat button, right beside the Headlights button, removes the pinup he thoughtlessly " +
                        "taped overtop of it, and hits it, turning off " +
                        "his headlights. \"----!\" he exclaims as he jabs again, this time ejecting " +
                        "and is just barely able to land on his feet on a nearby rooftop in a red light " +
                        "district.  As he shakes off dizziness and watches what's left of his very " +
                        "expensive flying car crash and burn somewhere in the city, he says, \"Those " +
                        "alien -------- are gonna to pay for shooting up my ride!\"</color>\n\n<color=lime>Press any key to continue...</color>\n";

        if (Input.anyKeyDown)
        {
            Tick();
            DukeLocation = States.rooftop;
        }
    }

    void Rooftop()
    {
        text.text = "<color=yellow>Duke is standing on a gritty rooftop of what passes for a high rise in Los Angeles.  " +
                "The roof is surrounded by a ledge with a chain-link fence preventing him from simply jumping off.  " +
                "A partly-burned \"Hollywood\" sign is visible on a hill.  </color>\n";

    }

    #endregion
    void Stub()
    {
        text.text = "<color=red>You are in a maze of twisty passages, all alike.</color>";
        print("In a stub state...this should never happen.");
    }
}
