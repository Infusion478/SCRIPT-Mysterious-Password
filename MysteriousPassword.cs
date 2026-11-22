using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class MysteriousPassword : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   public KMRuleSeedable RuleSeedable;

   private KMAudio.KMAudioRef soundRef;

   public KMSelectable[] buttons;
   public TextMesh[] text;
   public TextMesh lowerText;
   public AudioClip solveSound;

   private string[][] allWords = new string[][]
   {
      new string[] {"ABBEY", "ABIDE", "ABORT", "ABOUT", "ABOVE", "ABUSE", "ABYSS", "ACIDS", "ACORN", "ACRES", "ACTED", "ACTOR", "ACUTE", "ADPOT", "ADDED", "ADDER", "ADMIN", "ADMIT", "ADOPT", "ADORE", "ADORN", "ADULT", "AFFIX", "AFTER", "AGAIN", "AGENT", "AGILE", "AGING", "AGONY", "AGREE", "AHEAD", "AIDED", "AIMED", "AIOLI", "AIRED", "AISLE", "ALARM", "ALBUM", "ALERT", "ALGAE", "ALIAS", "ALIBI", "ALIEN", "ALIGN", "ALIKE", "ALIVE", "ALLAY", "ALLEY", "ALLOT", "ALLOW", "ALLOY", "ALOFT", "ALONE", "ALONG", "ALOUD", "ALPHA", "ALTAR", "ALTER", "AMASS", "AMAZE", "AMBER", "AMBLE", "AMINO", "AMEND", "AMISS", "AMONG", "AMPLE", "AMUSE", "ANGEL", "ANGER", "ANGLE", "ANGRY", "ANGST", "ANIME", "ANION", "ANKLE", "ANNEX", "ANNOY", "ANNUL", "ANTIC", "ANVIL", "AORTA", "APART", "APPLE", "APPLY", "APRON", "AREAS", "ARENA", "ARGON", "ARGUE", "ARISE", "ARMED", "ARMOR", "AROMA", "AROSE", "ARRAY", "ARROW", "ASHES", "ASIDE", "ASKED", "ASSET", "ATOLL", "ATOMS", "ATONE", "ATTIC", "AUDIO", "AUDIT", "AVAIL", "AVIAN", "AVOID", "AWAIT", "AWAKE", "AWARD", "AWARE", "AWASH", "AWFUL", "AXIAL", "AXIOM", "AZTEC"},
      new string[] {"BACKS", "BACON", "BADGE", "BADLY", "BAKED", "BAKER", "BANDS", "BANKS", "BARGE", "BASED", "BASES", "BASIC", "BASIL", "BASIN", "BASIS", "BATCH", "BEACH", "BEADS", "BEAMS", "BEANS", "BEARD", "BEARS", "BEAST", "BEGAN", "BEGIN", "BEGUN", "BEING", "BELLS", "BELLY", "BELOW", "BELTS", "BENCH", "BERRY", "BIBLE", "BIDET", "BIKES", "BILLS", "BINDS", "BINGE", "BINGO", "BIOME", "BIRCH", "BIRDS", "BIRTH", "BISON", "BLACK", "BLADE", "BLAME", "BLAND", "BLANK", "BLAST", "BLAZE", "BLEED", "BLEND", "BLESS", "BLIMP", "BLIND", "BLINK", "BLISS", "BLITZ", "BLOCK", "BLOOD", "BLOOM", "BLOWN", "BLOWS", "BLUES", "BLUFF", "BLUSH", "BOARD", "BOAST", "BOATS", "BOLTS", "BOMBS", "BONDS", "BONES", "BONUS", "BOOKS", "BOOST", "BOOTH", "BOOTS", "BORED", "BORON", "BOUND", "BOWLS", "BOXED", "BOXER", "BOXES", "BRACE", "BRAIN", "BRAKE", "BRAND", "BRASS", "BRAVE", "BRAWL", "BREAD", "BREAK", "BREED", "BRIBE", "BRICK", "BRIDE", "BRIEF", "BRING", "BRISK", "BROAD", "BROIL", "BROKE", "BROOK", "BROOM", "BROTH", "BROWN", "BRUSH", "BUDDY", "BUDGE", "BUILD", "BUILT", "BULBS", "BULGE", "BULKY", "BUMPY", "BUNCH", "BUNNY", "BURNS", "BURNT", "BURST", "BUSES", "BUZZY", "BYWAY"},
      new string[] {"CABIN", "CABLE", "CACHE", "CAKES", "CALLS", "CAMPS", "CANAL", "CANDY", "CANOE", "CANON", "CARDS", "CARED", "CARES", "CARGO", "CAROL", "CARRY", "CARVE", "CASED", "CASES", "CATCH", "CATER", "CAUSE", "CAVES", "CEASE", "CEDED", "CELLS", "CENTS", "CHAIN", "CHAIR", "CHALK", "CHAMP", "CHANT", "CHAOS", "CHARM", "CHART", "CHASE", "CHASM", "CHEAP", "CHEAT", "CHECK", "CHEER", "CHESS", "CHEST", "CHICK", "CHIEF", "CHILD", "CHILI", "CHILL", "CHIME", "CHIPS", "CHOIR", "CHORD", "CHORE", "CHOSE", "CHUNK", "CHUTE", "CIDER", "CITED", "CITES", "CIVIC", "CIVIL", "CLAIM", "CLASH", "CLASS", "CLAWS", "CLEAN", "CLEAR", "CLERK", "CLICK", "CLIFF", "CLIMB", "CLING", "CLOAK", "CLOCK", "CLONE", "CLOSE", "CLOTH", "CLOUD", "CLOUT", "CLOWN", "CLUBS", "CLUES", "CLUNG", "CLUNK", "COACH", "COAST", "COATS", "COCOA", "CODES", "COINS", "COLON", "COLOR", "COMES", "COMIC", "COMMA", "CORAL", "CORGI", "CORNY", "CORPS", "COSTS", "COUCH", "COUGH", "COULD", "COUNT", "COURT", "COVEN", "COVER", "CRACK", "CRAFT", "CRANE", "CRANK", "CRASH", "CRASS", "CRATE", "CRAVE", "CRAWL", "CRAZY", "CREAK", "CREAM", "CREED", "CREEK", "CREPT", "CREST", "CREWS", "CRIED", "CRIES", "CRIME", "CRISP", "CROPS", "CROSS", "CROWD", "CROWN", "CRUEL", "CRUSH", "CRUST", "CRYPT", "CUBIC", "CURLS", "CURLY", "CURRY", "CURSE", "CURVE", "CYCLE"},
      new string[] {"DAILY", "DAIRY", "DAISY", "DANCE", "DARED", "DATED", "DATES", "DATUM", "DEALS", "DEALT", "DEATH", "DEBIT", "DEBTS", "DEBUG", "DEBUT", "DECAF", "DECAL", "DECAY", "DECOR", "DECOY", "DEEDS", "DEITY", "DELAY", "DELTA", "DELVE", "DENIM", "DENSE", "DEPOT", "DEPTH", "DERBY", "DESKS", "DETER", "DEUCE", "DEVIL", "DIARY", "DICED", "DICEY", "DIETS", "DIGIT", "DIMLY", "DINER", "DIRTY", "DISCO", "DISCS", "DISKS", "DITCH", "DIVED", "DIVER", "DIZZY", "DOCKS", "DODGE", "DOING", "DOLLS", "DONOR", "DONUT", "DOORS", "DOSED", "DOSES", "DOUBT", "DOUGH", "DOUSE", "DOZEN", "DRAFT", "DRAIN", "DRAMA", "DRANK", "DRAWN", "DRAWS", "DREAD", "DREAM", "DRESS", "DRIED", "DRIER", "DRIFT", "DRILL", "DRINK", "DRIVE", "DRONE", "DROPS", "DROVE", "DROWN", "DRUMS", "DRUNK", "DRYER", "DRYLY", "DUCKS", "DUMMY", "DUNES", "DUSTY", "DWARF", "DWELL", "DYING"},
      new string[] {"EAGER", "EAGLE", "EARLY", "EARTH", "EASED", "EASEL", "EATEN", "EDGES", "EERIE", "EIGHT", "ELBOW", "ELDER", "ELECT", "ELITE", "ELUDE", "ELVES", "EMOTE", "EMPTY", "ENACT", "ENDED", "ENEMY", "ENJOY", "ENSUE", "ENTER", "ENTRY", "EQUAL", "EQUIP", "ERASE", "ERROR", "ERUPT", "ESSAY", "EVADE", "EVENT", "EVERY", "EVICT", "EXACT", "EXAMS", "EXERT", "EXILE", "EXIST", "EXTRA", "EXUDE"},
      new string[] {"FACED", "FACES", "FACET", "FACTS", "FADED", "FAILS", "FAINT", "FAIRY", "FAITH", "FALLS", "FALSE", "FANCY", "FARCE", "FARES", "FARMS", "FATAL", "FAULT", "FAUNA", "FAVOR", "FEARS", "FEAST", "FEELS", "FEINT", "FENCE", "FERRY", "FETCH", "FEVER", "FEWER", "FIBER", "FIBRE", "FIELD", "FIERY", "FIFTH", "FIFTY", "FIGHT", "FILED", "FILES", "FILET", "FILLS", "FILMS", "FILTH", "FINAL", "FINDS", "FINED", "FINES", "FIRED", "FIRES", "FIRMS", "FIRST", "FISTS", "FIXED", "FLAGS", "FLAIR", "FLAME", "FLARE", "FLASH", "FLASK", "FLATS", "FLAWS", "FLESH", "FLIES", "FLING", "FLOAT", "FLOCK", "FLOOD", "FLOOR", "FLORA", "FLOUR", "FLOWN", "FLOWS", "FLUID", "FLUNG", "FLUSH", "FLUTE", "FOCAL", "FOCUS", "FOGGY", "FOLDS", "FOLKS", "FOLLY", "FONTS", "FOODS", "FOOLS", "FORCE", "FORGE", "FORMS", "FORTE", "FORTH", "FORTY", "FORUM", "FOUND", "FOURS", "FOXES", "FRAIL", "FRAME", "FRANK", "FRAUD", "FREAK", "FREED", "FRESH", "FRIED", "FROGS", "FRONT", "FROST", "FROWN", "FROZE", "FRUIT", "FUDGE", "FUELS", "FULLY", "FUMES", "FUNDS", "FUNNY", "FUSED", "FUZZY"},
      new string[] {"GAINS", "GAMES", "GANGS", "GASES", "GATES", "GAUGE", "GAZED", "GEESE", "GENES", "GENIE", "GENRE", "GERMS", "GHOST", "GHOUL", "GIANT", "GIFTS", "GIRLS", "GIRLY", "GIRTH", "GIVEN", "GIVES", "GIZMO", "GLARE", "GLASS", "GLEAM", "GLIDE", "GLINT", "GLOBE", "GLOOM", "GLORY", "GLOSS", "GLOVE", "GLUED", "GLYPH", "GOALS", "GOATS", "GOING", "GOODS", "GOOFY", "GOOSE", "GORGE", "GRACE", "GRADE", "GRAIN", "GRAMS", "GRAND", "GRANT", "GRAPE", "GRAPH", "GRASP", "GRASS", "GRATE", "GRAVE", "GRAVY", "GREAT", "GREED", "GREEK", "GREEN", "GREET", "GRIEF", "GRILL", "GRIME", "GRIMY", "GRIND", "GRIPS", "GROOM", "GROUP", "GROWN", "GROWS", "GRUNT", "GUARD", "GUAVA", "GUESS", "GUEST", "GUIDE", "GUILD", "GUILT", "GUMMY", "GUTSY"},
      new string[] {"HABIT", "HAIKU", "HAIRS", "HALLS", "HALVE", "HANDS", "HANDY", "HAPPY", "HARPS", "HARSH", "HASTE", "HASTY", "HATCH", "HATED", "HATES", "HAUNT", "HAVEN", "HAVOC", "HAZEL", "HEADS", "HEARD", "HEARS", "HEART", "HEAVE", "HEAVY", "HEDGE", "HEELS", "HEFTY", "HEIRS", "HEIST", "HELIX", "HELLO", "HELPS", "HENCE", "HERBS", "HERDS", "HIKED", "HIKER", "HIKES", "HILLS", "HINGE", "HINTS", "HIPPO", "HIRED", "HOBBY", "HOLDS", "HOLES", "HOLLY", "HOMES", "HONEY", "HONOR", "HOOKS", "HOPED", "HOPES", "HORSE", "HOTEL", "HOUND", "HOURS", "HOUSE", "HUMAN", "HUMID", "HUMOR", "HURRY", "HURTS", "HUSKY", "HYENA", "HYMNS", "HYPER"},
      new string[] {"ICHOR", "ICING", "ICONS", "IDEAL", "IDEAS", "IDIOM", "IDIOT", "IDLED", "IDYLL", "IGLOO", "IMAGE", "IMPLY", "INANE", "INCUR", "INDEX", "INERT", "INFER", "INGOT", "INLET", "INNER", "INPUT", "INTRO", "IRONY", "ISSUE", "ITCHY", "ITEMS", "IVORY"},
      new string[] {"JAUNT", "JEANS", "JELLY", "JEWEL", "JOINS", "JOINT", "JOKER", "JOKES", "JOLLY", "JOULE", "JOUST", "JUDGE", "JUICE", "JUICY", "JUMBO", "JUMPS", "JUMPY"},
      new string[] {"KANJI", "KAPUT", "KARMA", "KAYAK", "KAZOO", "KEBAB", "KEEPS", "KICKS", "KILLS", "KINDA", "KINDS", "KINGS", "KITTY", "KNAVE", "KNEAD", "KNEEL", "KNEES", "KNELT", "KNIFE", "KNOBS", "KNOCK", "KNOLL", "KNOTS", "KNOWN", "KNOWS", "KOALA", "KOOKY", "KUDOS"},
      new string[] {"LABEL", "LABOR", "LACED", "LACES", "LACKS", "LADLE", "LAKES", "LAMBS", "LAMPS", "LANDS", "LANES", "LAPSE", "LARGE", "LARVA", "LASER", "LASSO", "LASTS", "LATER", "LATTE", "LAUGH", "LAWNS", "LAYER", "LEADS", "LEAFY", "LEAKY", "LEAPT", "LEARN", "LEASE", "LEASH", "LEAST", "LEAVE", "LEDGE", "LEECH", "LEGAL", "LEMON", "LEMUR", "LEVEL", "LEVER", "LIBEL", "LIFTS", "LIGHT", "LIKED", "LIKES", "LILAC", "LIMBO", "LIMBS", "LIMIT", "LINED", "LINEN", "LINES", "LINKS", "LIONS", "LIPID", "LISTS", "LITER", "LITRE", "LIVED", "LIVER", "LIVES", "LLAMA", "LOADS", "LOANS", "LOBBY", "LOCAL", "LOCKS", "LODGE", "LOGIC", "LOOKS", "LOOPS", "LOOSE", "LORDS", "LOSER", "LOSES", "LOTUS", "LOUSE", "LOUSY", "LOVED", "LOVER", "LOVES", "LOWER", "LOYAL", "LUCID", "LUCKY", "LUMEN", "LUMPS", "LUMPY", "LUNAR", "LUNCH", "LUNGE", "LUNGS", "LYING", "LYRIC"},
      new string[] {"MACHO", "MADAM", "MADLY", "MAGIC", "MAGMA", "MAINS", "MAIZE", "MAJOR", "MAKER", "MAKES", "MANGO", "MANIA", "MANLY", "MANOR", "MAPLE", "MARCH", "MARKS", "MARRY", "MARSH", "MASKS", "MATCH", "MATES", "MATHS", "MATTE", "MAXIM", "MAYBE", "MAYOR", "MEALS", "MEANS", "MEANT", "MEATS", "MEATY", "MEDAL", "MEDIA", "MEDIC", "MEETS", "MELON", "MENUS", "MERCY", "MERGE", "MERIT", "MERRY", "MESSY", "METAL", "METER", "METRE", "MICRO", "MIDST", "MIGHT", "MILES", "MILLS", "MIMIC", "MINCE", "MINDS", "MINED", "MINER", "MINES", "MINOR", "MINTY", "MISTY", "MIXED", "MIXER", "MODEL", "MODEM", "MODES", "MOIST", "MOLAR", "MOLDY", "MOLES", "MONEY", "MONKS", "MONTH", "MOODS", "MOOSE", "MORAL", "MORAY", "MORPH", "MOTEL", "MOTOR", "MOTTO", "MOUND", "MOUNT", "MOUSE", "MOUTH", "MOVED", "MOVER", "MOVES", "MOVIE", "MUDDY", "MUMMY", "MUSIC", "MUTED", "MYTHS"},
      new string[] {"NACHO", "NAILS", "NAIVE", "NAMED", "NAMES", "NASAL", "NASTY", "NAVAL", "NECKS", "NEEDS", "NEEDY", "NERVE", "NESTS", "NEVER", "NEWER", "NEWLY", "NEXUS", "NICER", "NICHE", "NIECE", "NIFTY", "NIGHT", "NINJA", "NINTH", "NITRO", "NOBLE", "NODES", "NOISE", "NOISY", "NOMAD", "NORMS", "NORTH", "NOSES", "NOTCH", "NOTED", "NOTES", "NOVEL", "NUDGE", "NURSE", "NUTTY", "NYLON", "NYMPH"},
      new string[] {"OASIS", "OCCUR", "OCEAN", "OCTAL", "ODDLY", "OFFER", "OFTEN", "OLDER", "OLIVE", "ONION", "ONSET", "OPENS", "OPERA", "OPTIC", "ORBIT", "ORDER", "ORGAN", "OTHER", "OTTER", "OUGHT", "OUNCE", "OUTDO", "OUTER", "OWNED", "OWNER", "OXIDE", "OZONE"},
      new string[] {"PACKS", "PAGES", "PAINS", "PAINT", "PAIRS", "PALMS", "PANDA", "PANEL", "PANIC", "PANTS", "PAPER", "PARKS", "PARTS", "PARTY", "PASTA", "PASTE", "PATCH", "PATHS", "PATIO", "PAUSE", "PEACE", "PEACH", "PEAKS", "PEARL", "PEARS", "PEDAL", "PEERS", "PENCE", "PENNY", "PERIL", "PESTS", "PETTY", "PHASE", "PHONE", "PHOTO", "PIANO", "PIECE", "PILED", "PILES", "PILLS", "PILOT", "PINCH", "PINTS", "PIPES", "PITCH", "PIVOT", "PIXEL", "PIXIE", "PIZZA", "PLACE", "PLAIN", "PLANE", "PLANK", "PLANS", "PLANT", "PLATE", "PLAYS", "PLAZA", "PLEAD", "PLOTS", "PLUMP", "POEMS", "POETS", "POINT", "POKER", "POLAR", "POLES", "POLIO", "POLLS", "PONDS", "POOLS", "PORCH", "PORES", "PORTS", "POSED", "POSES", "POSTS", "POUCH", "POUND", "POWER", "PRESS", "PRICE", "PRICY", "PRIDE", "PRINT", "PRIOR", "PRISM", "PRIZE", "PROBE", "PROMO", "PRONE", "PROOF", "PROSE", "PROUD", "PROVE", "PROXY", "PRUDE", "PRUNE", "PULLS", "PULSE", "PUMPS", "PUNCH", "PUPIL", "PUPPY", "PURSE", "PYLON"},
      new string[] {"QUAIL", "QUARK", "QUART", "QUASI", "QUEEN", "QUERY", "QUEST", "QUEUE", "QUICK", "QUIET", "QUILL", "QUILT", "QUIRK", "QUITE", "QUOTA", "QUOTE"},
      new string[] {"RACED", "RACES", "RADAR", "RADIO", "RAIDS", "RAILS", "RAINY", "RAISE", "RALLY", "RAMPS", "RANCH", "RANGE", "RANKS", "RAPID", "RATED", "RATES", "RATIO", "RAVEN", "RAZOR", "REACH", "REACT", "READS", "READY", "REALM", "REBEL", "RECAP", "REFER", "REIGN", "RELAX", "RELAY", "RELIC", "REMIX", "RENEW", "RENTS", "REPAY", "REPLY", "RERUN", "RESIN", "RESTS", "RETRO", "REUSE", "RHINO", "RHYME", "RIDER", "RIDGE", "RIFLE", "RIGHT", "RIGID", "RIGOR", "RINGS", "RINSE", "RIOTS", "RISEN", "RISES", "RISKS", "RISKY", "RITES", "RIVAL", "RIVER", "ROADS", "ROAST", "ROBES", "ROBOT", "ROCKS", "ROCKY", "ROGUE", "ROLES", "ROLLS", "ROOMS", "ROOTS", "ROPES", "ROSES", "ROSIN", "ROTOR", "ROUGH", "ROUND", "ROUTE", "ROVER", "ROWED", "ROYAL", "RUGBY", "RUINS", "RULED", "RULER", "RULES", "RUMOR", "RUNIC", "RURAL", "RUSTY"},
      new string[] {"SADLY", "SAFER", "SAILS", "SAINT", "SALAD", "SALES", "SALON", "SALSA", "SALTS", "SALTY", "SANDY", "SAUCE", "SAUCY", "SAUNA", "SAVED", "SAVES", "SAVOR", "SAVVY", "SCALD", "SCALE", "SCALP", "SCARE", "SCARF", "SCARS", "SCARY", "SCENE", "SCENT", "SCOFF", "SCOLD", "SCONE", "SCOOP", "SCOOT", "SCOPE", "SCORE", "SCORN", "SCOUR", "SCOUT", "SCRAP", "SCREW", "SCRUB", "SCUBA", "SEALS", "SEAMS", "SEATS", "SEEDS", "SEEKS", "SEEMS", "SEGUE", "SEIZE", "SELLS", "SENDS", "SENSE", "SERUM", "SERVE", "SETUP", "SEVEN", "SHADE", "SHADY", "SHAFT", "SHAKE", "SHAKY", "SHALL", "SHAME", "SHANK", "SHAPE", "SHARD", "SHARE", "SHARP", "SHAVE", "SHEAR", "SHEEP", "SHEER", "SHEET", "SHELF", "SHELL", "SHIFT", "SHINE", "SHINY", "SHIPS", "SHIRT", "SHOCK", "SHOES", "SHONE", "SHOOK", "SHOOT", "SHOPS", "SHORE", "SHORT", "SHOTS", "SHOUT", "SHOVE", "SHOWN", "SHOWS", "SHRUG", "SHUSH", "SHYLY", "SIDES", "SIEGE", "SIEVE", "SIGHT", "SIGIL", "SIGNS", "SILLY", "SINCE", "SINGE", "SINGS", "SITAR", "SITES", "SIXTH", "SIXTY", "SIZED", "SIZES", "SKATE", "SKIER", "SKIES", "SKILL", "SKINS", "SKIRT", "SKULL", "SLABS", "SLAIN", "SLANG", "SLANT", "SLASH", "SLATE", "SLEEK", "SLEEP", "SLEET", "SLEPT", "SLICE", "SLIDE", "SLIME", "SLIMY", "SLING", "SLOPE", "SLOTH", "SLOTS", "SLUSH", "SLYLY", "SMALL", "SMART", "SMASH", "SMEAR", "SMELL", "SMELT", "SMILE", "SMITE", "SMOKE", "SNAIL", "SNAKE", "SNARE", "SNIFF", "SNIPE", "SNORE", "SNORT", "SNOUT", "SOBER", "SOCKS", "SOGGY", "SOILS", "SOLAR", "SOLID", "SOLVE", "SONAR", "SONGS", "SONIC", "SORRY", "SORTS", "SOULS", "SOUND", "SOUTH", "SPACE", "SPADE", "SPARE", "SPARK", "SPAWN", "SPEAK", "SPEED", "SPELL", "SPEND", "SPENT", "SPIES", "SPINE", "SPLAT", "SPLIT", "SPOIL", "SPOKE", "SPOON", "SPORT", "SPOTS", "SPRAY", "SPURS", "SQUAD", "STACK", "STAFF", "STAGE", "STAIN", "STAIR", "STAKE", "STALE", "STALL", "STAMP", "STAND", "STARE", "STARS", "START", "STASH", "STATE", "STAYS", "STEAK", "STEAL", "STEAM", "STEEL", "STEEP", "STEER", "STEMS", "STEPS", "STERN", "STICK", "STIFF", "STILL", "STILT", "STING", "STINK", "STOCK", "STOIC", "STOLE", "STOMP", "STONE", "STONY", "STOOD", "STOOL", "STOPS", "STORE", "STORM", "STORY", "STOUT", "STOVE", "STRAP", "STRAW", "STRAY", "STRIP", "STRUM", "STRUT", "STUCK", "STUDY", "STUFF", "STUMP", "STUNT", "STYLE", "SUGAR", "SUITE", "SUITS", "SUNNY", "SUPER", "SURGE", "SUSHI", "SWAMP", "SWANS", "SWARM", "SWEAT", "SWEEP", "SWEET", "SWELL", "SWEPT", "SWIFT", "SWILL", "SWINE", "SWING", "SWIPE", "SWIRL", "SWORD", "SWUNG"},
      new string[] {"TABLE", "TAFFY", "TAILS", "TAKEN", "TAKES", "TALES", "TALKS", "TALLY", "TALON", "TAMED", "TANGO", "TANGY", "TANKS", "TAPES", "TAPER", "TARDY", "TASKS", "TASTE", "TASTY", "TAUNT", "TAXED", "TAXES", "TAXIS", "TEACH", "TEAMS", "TEARS", "TEARY", "TECHY", "TEENS", "TEENY", "TEETH", "TELLS", "TEMPO", "TENDS", "TENOR", "TENSE", "TENTH", "TENTS", "TERMS", "TESTS", "TEXTS", "THANK", "THEFT", "THEIR", "THEME", "THERE", "THESE", "THETA", "THICK", "THIEF", "THIGH", "THING", "THINK", "THIRD", "THORN", "THOSE", "THREE", "THREW", "THROW", "THUMB", "TIDAL", "TIDES", "TIGER", "TIGHT", "TILDE", "TILED", "TILES", "TIMED", "TIMER", "TIMES", "TIMID", "TINNY", "TIPSY", "TIRED", "TIRES", "TITLE", "TOAST", "TODAY", "TOKEN", "TONAL", "TONED", "TONES", "TONGS", "TONIC", "TOOLS", "TOONS", "TOOTH", "TOPAZ", "TOPIC", "TORCH", "TORSO", "TORUS", "TOTAL", "TOTEM", "TOUCH", "TOUGH", "TOURS", "TOWEL", "TOWER", "TOWNS", "TOXIC", "TOXIN", "TRACE", "TRACK", "TRACT", "TRADE", "TRAIL", "TRAIN", "TRAIT", "TRAMP", "TRAMS", "TRASH", "TRAYS", "TREAD", "TREAT", "TREES", "TREND", "TRIAD", "TRIAL", "TRIBE", "TRICK", "TRIED", "TRIES", "TRIKE", "TRILL", "TRIPS", "TRITE", "TROLL", "TROOP", "TROUT", "TRUCE", "TRUCK", "TRULY", "TRUNK", "TRUST", "TRUTH", "TUBES", "TULIP", "TUNED", "TUNES", "TUNIC", "TURNS", "TUTOR", "TWEAK", "TWICE", "TWINS", "TWIRL", "TWIST", "TYING", "TYPES", "TYRES"},
      new string[] {"UDDER", "ULCER", "ULTRA", "UMBRA", "UNBAN", "UNCAP", "UNCLE", "UNCUT", "UNDER", "UNDUE", "UNFIT", "UNIFY", "UNION", "UNITE", "UNITS", "UNITY", "UNLIT", "UNMET", "UNTIE", "UNTIL", "UNZIP", "UPPER", "UPSET", "URBAN", "URGED", "USAGE", "USERS", "USHER", "USING", "USUAL", "UTTER", "UVULA"},
      new string[] {"VAGUE", "VALID", "VALOR", "VALUE", "VALVE", "VAPOR", "VAULT", "VAUNT", "VEINS", "VENOM", "VENUE", "VERBS", "VERGE", "VERSE", "VIDEO", "VIEWS", "VIGIL", "VIGOR", "VILLA", "VINES", "VINYL", "VIRAL", "VIRUS", "VISIT", "VISOR", "VITAL", "VIVID", "VIXEN", "VOCAL", "VOGUE", "VOICE", "VOILA", "VOMIT", "VOTED", "VOTER", "VOTES", "VOUCH", "VOWED", "VOWEL"},
      new string[] {"WAGES", "WAGON", "WAIST", "WAITS", "WAIVE", "WALKS", "WALLS", "WALTZ", "WANTS", "WARNS", "WASTE", "WATCH", "WATER", "WAVED", "WAVES", "WEARS", "WEARY", "WEAVE", "WEDGE", "WEEKS", "WEIGH", "WEIRD", "WELLS", "WHALE", "WHEAT", "WHEEL", "WHERE", "WHICH", "WHILE", "WHINE", "WHISK", "WHITE", "WHOLE", "WHOSE", "WIDEN", "WIDER", "WIDOW", "WIDTH", "WIELD", "WILLS", "WINDS", "WINDY", "WINGS", "WIPED", "WIRED", "WIRES", "WISER", "WITCH", "WITTY", "WIVES", "WOMAN", "WOMEN", "WOODS", "WORDS", "WORKS", "WORLD", "WORMS", "WORMY", "WORRY", "WORSE", "WORST", "WORTH", "WOULD", "WOUND", "WOVEN", "WRATH", "WRECK", "WRIST", "WRITE", "WRONG", "WROTE", "WRYLY"},
      new string[] {"XEBEC", "XENIA", "XENON", "XERIC", "XEROX", "XYLEM", "XYLYL", "XYSTS"},
      new string[] {"YACHT", "YARDS", "YAWNS", "YEARN", "YEARS", "YEAST", "YELLS", "YIELD", "YODEL", "YOUNG", "YOURS", "YOUTH", "YUMMY"},
      new string[] {"ZAPPY", "ZEBRA", "ZEROS", "ZESTS", "ZESTY", "ZILCH", "ZIPPY", "ZONAL", "ZONED", "ZONES", "ZOOMS"}
   };
   private List<string> words = new List<string> {"LUCKY", "FARCE", "VOILA", "YAWNS", "FOCUS", "AXIOM", "NAMED", "XEROX", "JUMPY", "PROXY", "SHIFT", "YIELD", "KOALA", "QUILT", "APPLY", "LEVER", "SMALL", "BROOK", "EQUIP", "OCEAN", "BREAK", "DIGIT", "EXERT", "CHART", "RINGS", "QUARK", "ULTRA", "IGLOO", "VALUE", "COAST", "TOPAZ", "WHERE", "DRINK", "KAPUT", "HEDGE", "COLOR", "XERIC", "JAUNT", "UNDER", "DICEY", "ENSUE", "LINEN", "OFTEN", "IDEAS", "ZOOMS", "INNER", "ZILCH", "PRIZE", "NOBLE", "ERUPT", "VINYL", "TRICK", "FAVOR", "QUASI", "DELTA", "DRIVE", "PHASE", "WORDS", "GLYPH", "FRONT", "HAZEL", "QUEUE", "MAIZE", "BLACK", "IVORY", "UMBRA", "KNOLL", "XENON", "WHITE", "VOMIT", "GREAT", "BEAST", "ARGON", "GUILT", "KNOBS", "YOUTH", "CARRY", "STRIP", "INCUR", "USUAL", "MERCY", "FOUND", "TAPER", "HATCH", "OUTER", "POUND", "GERMS", "HYPER", "EXTRA", "MAJOR", "XEBEC", "RERUN", "NEVER", "MINES", "SPEAK", "WATER", "OZONE", "HOUSE", "JELLY", "OCTAL", "NOVEL", "GIANT", "YACHT", "QUOTE", "BINDS", "SUPER", "KOOKY", "UNTIL", "ZESTY", "YEAST", "VALID", "JEWEL", "AGAIN", "LARGE", "TAUNT", "JOUST", "TEMPO", "NOISY", "WINDY", "RAPID", "AISLE", "ZONAL", "ROWED", "CRYPT", "PUPIL", "RATIO", "LOGIC", "XYSTS", "ZEBRA", "MAXIM"};
   private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
   private int visibleIndex, hiddenIndex;
   private string visiblePassword, hiddenPassword;
   private string[] visibleOptions, hiddenOptions;
   private int[][] visibleIndices, hiddenIndices;
   private int[] displayIndices = new int[5];
   private string casing;
   private string answer;
   private int defocusCount;
   private bool ready, focused;
   private bool focusing = true;

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;
   private bool TwitchPlaysActive;

   void Awake() {
      ModuleId = ModuleIdCounter++;
      GetComponent<KMSelectable>().OnFocus += delegate () { focused = true; };
      GetComponent<KMSelectable>().OnDefocus += delegate () { focused = false; };
      foreach (KMSelectable button in buttons)
         button.OnInteract += delegate () { ButtonPress(button); return false; };
   }

   void ButtonPress(KMSelectable button) {
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, button.transform);
      button.AddInteractionPunch(.5f);
      if (ModuleSolved)
         return;
      for (int i = 0; i < 5; i++) {
         if (button == buttons[i]) {
            displayIndices[i]++;
            displayIndices[i] %= 5;
            text[i].text = visibleOptions[i][displayIndices[i]].ToString();
         }
      }
   }

   void OnDestroy() {
      ready = false;
      StopFlicker();
   }

   void Start() {
      // RULE SEED
      var rnd = RuleSeedable.GetRNG();
      if (rnd.Seed != 1) {
         Debug.LogFormat("[Mysterious Password #{0}] Ooh, you're using rule seed {1}...", ModuleId, rnd.Seed);
         words.Clear();
         for (int i = 0; i < 26; i++) {
            rnd.ShuffleFisherYates(allWords[i]);
            words.AddRange(allWords[i].Take(5));
         }
         rnd.ShuffleFisherYates(words);
      }
      visibleIndex = Rnd.Range(0, 130);
      do hiddenIndex = Rnd.Range(0, 130); while (visibleIndex == hiddenIndex);
      visiblePassword = words[visibleIndex];
      hiddenPassword = words[hiddenIndex];
      Debug.LogFormat("[Mysterious Password #{0}] The visible password is {1}, and the mysterious password is {2}...", ModuleId, visiblePassword, hiddenPassword);
      Debug.LogFormat("[Mysterious Password #{0}] The visible password is in column {1} and row {2}...", ModuleId, visibleIndex % 5, visibleIndex / 5);
      visibleOptions = new string[5];
      visibleIndices = new int[5][] {new int[5], new int[5], new int[5], new int[5], new int[5]};
      visibleRetry:
      for (int i = 0; i < 5; i++) {
         List<int> choices = Enumerable.Range(0, 26).ToList().Shuffle();
         visibleOptions[i] = visiblePassword[i].ToString();
         choices.Remove(alphabet.IndexOf(visiblePassword[i]));
         int[] chosen = choices.Take(5).ToArray();
         chosen.Shuffle();
         for (int j = 0; j < 4; j++)
            visibleOptions[i] += alphabet[chosen[j]];
         visibleOptions[i] = Shuffle(visibleOptions[i]);
         for (int j = 0; j < 4; j++)
            visibleIndices[i][j] = alphabet.IndexOf(visibleOptions[i][j]);
      }
      if (words.Any(w => w != visiblePassword && Enumerable.Range(0, 5).All(i => visibleIndices[i].Contains(w[i] - 'A'))))
         goto visibleRetry;
      hiddenOptions = new string[5];
      hiddenIndices = new int[5][] {new int[5], new int[5], new int[5], new int[5], new int[5]};
      hiddenRetry:
      for (int i = 0; i < 5; i++) {
         List<int> choices = Enumerable.Range(0, 26).ToList().Shuffle();
         hiddenOptions[i] = hiddenPassword[i].ToString();
         choices.Remove(alphabet.IndexOf(hiddenPassword[i]));
         int[] chosen = choices.Take(5).ToArray();
         chosen.Shuffle();
         for (int j = 0; j < 4; j++)
            hiddenOptions[i] += alphabet[chosen[j]];
         hiddenOptions[i] = Shuffle(hiddenOptions[i]);
         for (int j = 0; j < 4; j++)
            hiddenIndices[i][j] = alphabet.IndexOf(hiddenOptions[i][j]);
      }
      if (words.Any(w => w != hiddenPassword && Enumerable.Range(0, 5).All(i => hiddenIndices[i].Contains(w[i] - 'A'))))
         goto hiddenRetry;
      Debug.LogFormat("[Mysterious Password #{0}] The options for the visible password (for each column) are: {1}...", ModuleId, Enumerable.Range(0, 5).Select(x => visibleOptions[x].Join("")).Join("; "));
      Debug.LogFormat("[Mysterious Password #{0}] The options for the mysterious password (for each column) are: {1}...", ModuleId, Enumerable.Range(0, 5).Select(x => hiddenOptions[x].Join("")).Join("; "));
      for (int i = 0; i < 5; i++)
         text[i].text = visibleOptions[i][0].ToString();
      ready = true;
   }

   string Shuffle(string input) {
      char[] chars = input.ToCharArray();
      for (int i = chars.Length - 1; i > 0; i--) {
         int j = Rnd.Range(0, i);
         char temp = chars[i];
         chars[i] = chars[j];
         chars[j] = temp;
      }
      return new string(chars);
   }

   void Solve() {
      ModuleSolved = true;
      GUIUtility.systemCopyBuffer = "The mysterious password has been cracked...";
      GetComponent<KMBombModule>().HandlePass();
      StopFlicker();
      Audio.PlaySoundAtTransform("PS2Startup", transform);
      lowerText.text = "! ! !";
      StartCoroutine(FadeText());
   }

   void Strike() {
      GetComponent<KMBombModule>().HandleStrike();
      StopFlicker();
      focusing = true;
      for (int i = 0; i < 5; i++)
         text[i].color = new Color32(38, 92, 16, 255);
      do visibleIndex = Rnd.Range(0, 130); while (visibleIndex == hiddenIndex);
      visiblePassword = words[visibleIndex];
      Debug.LogFormat("[Mysterious Password #{0}] The visible password is now {1}...", ModuleId, visiblePassword);
      Debug.LogFormat("[Mysterious Password #{0}] The visible password is now in column {1} and row {2}...", ModuleId, visibleIndex % 5, visibleIndex / 5);
      visibleOptions = new string[5];
      visibleIndices = new int[5][] {new int[5], new int[5], new int[5], new int[5], new int[5]};
      resetRetry:
      for (int i = 0; i < 5; i++) {
         List<int> choices = Enumerable.Range(0, 26).ToList().Shuffle();
         visibleOptions[i] = visiblePassword[i].ToString();
         choices.Remove(alphabet.IndexOf(visiblePassword[i]));
         int[] chosen = choices.Take(5).ToArray();
         chosen.Shuffle();
         for (int j = 0; j < 4; j++)
            visibleOptions[i] += alphabet[chosen[j]];
         visibleOptions[i] = Shuffle(visibleOptions[i]);
         for (int j = 0; j < 4; j++)
            visibleIndices[i][j] = alphabet.IndexOf(visibleOptions[i][j]);
      }
      if (words.Any(w => w != visiblePassword && Enumerable.Range(0, 130).All(i => visibleIndices[i].Contains(w[i]))))
         goto resetRetry;
      Debug.LogFormat("[Mysterious Password #{0}] The options for the new visible password (for each column) are: {1}...", ModuleId, Enumerable.Range(0, 5).Select(x => visibleOptions[x].Join("")).Join("; "));
      for (int i = 0; i < 5; i++) {
         text[i].text = visibleOptions[i][0].ToString();
         displayIndices[i] = 0;
      }
   }

   void OnApplicationFocus(bool focus) {
      if (!ready || !focused || ModuleSolved || TwitchPlaysActive)
         return;
      focusing = focus;
      FocusChange();
   }

   void FocusChange() {
      if (focusing) {
         Debug.LogFormat("[Mysterious Password #{0}] You have focused back into the game...", ModuleId);
         StopFlicker();
         for (int i = 0; i < 5; i++)
            text[i].color = new Color32(38, 92, 16, 255);
         if (defocusCount < 6) {
            for (int i = 0; i < 5; i++)
               text[i].text = visibleOptions[i][displayIndices[i]].ToString();
         } else
            Submit();
      } else {
         defocusCount++;
         Debug.LogFormat("[Mysterious Password #{0}] You are now out of the game...", ModuleId);
         if (defocusCount < 6) {
            soundRef = Audio.PlaySoundAtTransformWithRef("Buzzing", transform);
            Debug.LogFormat("[Mysterious Password #{0}] Defocus number {1} shows {2}...", ModuleId, defocusCount, hiddenOptions[defocusCount - 1]);
            for (int i = 0; i < 5; i++) {
               text[i].text = hiddenOptions[defocusCount - 1][i].ToString();
               lowerText.text = "---";
               StartCoroutine(FlickerText(text[i], new Color32(20, 50, 8, 255), new Color32(38, 92, 16, 255)));
            }
         } else {
            soundRef = Audio.PlaySoundAtTransformWithRef("Electronic Buzzing", transform);
            casing = new[] {"UPPER", "LOWER", "TITLE"}[Rnd.Range(0, 3)];
            Debug.LogFormat("[Mysterious Password #{0}] Sixth defocus; it is showing {1}...", ModuleId, casing);
            answer = hiddenPassword;
            char[] shifted = new char[5];
            for (int i = 0; i < 5; i++)
               shifted[i] = alphabet[(Array.IndexOf(alphabet.ToCharArray(), answer[(i + visibleIndex) % 5]) + visibleIndex / 5) % 26];
            answer = new string(shifted);
            switch (casing) {
               case "UPPER": break;
               case "LOWER": answer = answer.ToLowerInvariant(); break;
               case "TITLE":
                  string temp = answer;
                  answer = temp[0].ToString();
                  for (int i = 1; i < 5; i++)
                     answer += temp[i].ToString().ToLowerInvariant();
                  break;
            }
            Debug.LogFormat("[Mysterious Password #{0}] The answer to submit is {1}...", ModuleId, answer);
            for (int i = 0; i < 5; i++) {
               text[i].text = casing[i].ToString();
               lowerText.text = "===";
               StartCoroutine(FlickerText(text[i], new Color32(38, 92, 16, 255), new Color32(72, 169, 40, 255)));
            }
         }
      }
   }

   void Submit(string query = "") {
      var clipboardText = "";
      if (string.IsNullOrEmpty(query))
         clipboardText = GUIUtility.systemCopyBuffer.Trim();
      else
         clipboardText = query.Trim();
      Debug.LogFormat("[Mysterious Password #{0}] You submitted: {1}", ModuleId, clipboardText);
      if (clipboardText == answer) {
         Debug.LogFormat("[Mysterious Password #{0}] Correct! Module solved!", ModuleId);
         for (int i = 0; i < 5; i++)
            text[i].text = hiddenPassword[i].ToString();
         Solve();
      } else {
         Debug.LogFormat("[Mysterious Password #{0}] That is wrong. Striking and changing visible password...", ModuleId);
         lowerText.text = "???";
         defocusCount = 0;
         Strike();
      }
   }

   void StopFlicker() {
      StopAllCoroutines();
      if (soundRef != null) {
         soundRef.StopSound();
         soundRef = null;
      }
      lowerText.text = "???";
   }

   IEnumerator FlickerText(TextMesh text, Color dark, Color light) {
      float time = 0f;
      while (true) {
          float t = Mathf.Abs(Mathf.Sin(time));
          text.color = Color.Lerp(dark, light, t);
          yield return null;
          time += Time.deltaTime * 5f;
      }
   }

   IEnumerator FadeText() {
      float duration = 3f;
      float elapsed = 0f;
      while (elapsed < duration) {
         for (int i = 0; i < 5; i++)
            text[i].color = new Color32(38, 92, 16, (byte)Easing.OutCubic(elapsed, 255, 0, duration));
         yield return null;
         elapsed += Time.deltaTime;
      }
   }

   // A lot of this is copied from the Twitch Plays support for Password (not affiliated with the original game)

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"!{0} cycle 1 3 5 [cycle through the letters in columns 1, 3, and 5] | !{0} toggle [move all columns up one letter] | !{0} focus/defocus [focuses/defocuses the game] | !{0} submit iNpUt [submits an answer]";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand(string Command) {
      Match m;
      yield return null;
      if (Regex.IsMatch(Command, @"^\s*toggle\s*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)) {
         if (!focusing) {
            yield return "sendtochaterror The module is not focused.";
            yield break;
         }
			for (int i = 0; i < 5; i++)
				buttons[i].OnInteract();
		} else if ((m = Regex.Match(Command, @"^\s*cycle\s+([ \d]+)\s*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)).Success) {
         if (!focusing) {
            yield return "sendtochaterror The module is not focused.";
            yield break;
         }
         var slots = new HashSet<int>();
			foreach (var piece in m.Groups[1].Value.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)) {
            int i;
				if (!int.TryParse(piece, out i) || i < 1 || i > 5)
				{
					yield return string.Format("sendtochaterror “{0}” is not a number from 1 to 5.", piece);
					yield break;
				}
				slots.Add(i);
			}
			if (slots.Count > 0) {
				foreach (var slot in slots)
				{
		         for (int hitCount = 0; hitCount < 5; ++hitCount) {
		         	buttons[slot - 1].OnInteract();
		         	yield return "trywaitcancel 1.0";
		         }
				}
			}
      } else if (Regex.IsMatch(Command, @"^\s*focus\s*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)) {
         if (focusing) {
            yield return "sendtochaterror The module is already focused.";
            yield break;
         } else if (defocusCount == 6) {
            yield return "sendtochaterror You need to submit something.";
            yield break;
         }
         focusing = true;
         FocusChange();
      } else if (Regex.IsMatch(Command, @"^\s*defocus\s*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)) {
         if (!focusing) {
            yield return "sendtochaterror The module is already defocused.";
            yield break;
         }
         focusing = false;
         FocusChange();
      } else if (Regex.IsMatch(Command, @"^\s*submit\s+([A-Z]|[a-z])+\s*$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)) {
         if (defocusCount < 6) {
            yield return "sendtochaterror The module is not ready for input yet.";
            yield break;
         }
         string[] Commands = Command.Split(' ');
         Submit(Commands[1]);
      } else {
         yield return null;
         yield return "sendtochaterror Invalid command.";
         yield break;
      }
   }

   IEnumerator TwitchHandleForcedSolve() {
      if (!focusing) {
         focusing = true;
         FocusChange();
      }
      for (int i = defocusCount; i < 5; i++) {
         focusing = false;
         FocusChange();
         focusing = true;
         FocusChange();
      }
      focusing = false;
      FocusChange();
      Submit(answer);
      yield return null;
   }
}
