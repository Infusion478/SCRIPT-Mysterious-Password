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

   private KMAudio.KMAudioRef soundRef;

   public KMSelectable[] buttons;
   public TextMesh[] text;
   public TextMesh lowerText;
   public AudioClip solveSound;

   private string[] words = {"LUCKY", "FARCE", "VOILA", "YAWNS", "FOCUS", "AXIOM", "NAMED", "XEROX", "JUMPY", "PROXY", "SHIFT", "YIELD", "KOALA", "QUILT", "APPLY", "LEVER", "SMALL", "BROOK", "EQUIP", "OCEAN", "BREAK", "DIGIT", "EXERT", "CHART", "RINGS", "QUARK", "ULTRA", "IGLOO", "VALUE", "COAST", "TOPAZ", "WHERE", "DRINK", "KAPUT", "HEDGE", "COLOR", "XERIC", "JAUNT", "UNDER", "DICEY", "ENSUE", "LINEN", "OFTEN", "IDEAS", "ZOOMS", "INNER", "ZILCH", "PRIZE", "NOBLE", "ERUPT", "VINYL", "TRICK", "FAVOR", "QUASI", "DELTA", "DRIVE", "PHASE", "WORDS", "GLYPH", "FRONT", "HAZEL", "QUEUE", "MAIZE", "BLACK", "IVORY", "UMBRA", "KNOLL", "XENON", "WHITE", "VOMIT", "GREAT", "BEAST", "ARGON", "GUILT", "KNOBS", "YOUTH", "CARRY", "STRIP", "INCUR", "USUAL", "MERCY", "FOUND", "TAPER", "HATCH", "OUTER", "POUND", "GERMS", "HYPER", "EXTRA", "MAJOR", "XEBEC", "RERUN", "NEVER", "MINES", "SPEAK", "WATER", "OZONE", "HOUSE", "JELLY", "OCTAL", "NOVEL", "GIANT", "YACHT", "QUOTE", "BINDS", "SUPER", "KOOKY", "UNTIL", "ZESTY", "YEAST", "VALID", "JEWEL", "AGAIN", "LARGE", "TAUNT", "JOUST", "TEMPO", "NOISY", "WINDY", "RAPID", "AISLE", "ZONAL", "ROWED", "CRYPT", "PUPIL", "RATIO", "LOGIC", "XYSTS", "ZEBRA", "MAXIM"};
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
      if (words.Any(w => w != visiblePassword && Enumerable.Range(0, 130).All(i => visibleIndices[i].Contains(w[i]))))
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
      if (words.Any(w => w != hiddenPassword && Enumerable.Range(0, 130).All(i => hiddenIndices[i].Contains(w[i]))))
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
