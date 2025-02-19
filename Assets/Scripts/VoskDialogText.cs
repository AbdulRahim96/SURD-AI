using UnityEngine;
using UnityEngine.UI;

public class VoskDialogText : MonoBehaviour 
{
    public VoskSpeechToText VoskSpeechToText;
	[TextArea(10,10)]
    public string DialogText;

    void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
    }

    private void OnTranscriptionResult(string obj)
    {
		// Save to file

        Debug.Log("from action callback:  " + obj);
        var result = new RecognitionResult(obj);
		print("Best Result: " + result.Phrases[0].Text);
		DialogText += result.Phrases[0].Text + " ";
        /*
        var result = new RecognitionResult(obj);
        foreach (RecognizedPhrase p in result.Phrases)
        {
			if (hi_regex.IsMatch(p.Text))
			{
				AddResponse("привет тебе");
				return;
			}
			if (who_regex.IsMatch(p.Text))
			{
				AddResponse("я робот учитель");
				return;
			}
			if (pass_regex.IsMatch(p.Text))
			{
                AddResponse("отлично");
				return;
			}
			if (help_regex.IsMatch(p.Text))
			{
				AddResponse("думай сам");
				return;
			}
			if (goat_back_regex.IsMatch(p.Text)) {
				if (goat_left == true) {
					AddResponse("коза ещё на левом берегу");
				} else if (man_left == true) {
					AddResponse("крестьянин ещё на левом берегу");
				} else {
					goat_left = true;
					man_left = true;
					CheckState();
				}
				return;
			}

			if (wolf_back_regex.IsMatch(p.Text)) {
				if (wolf_left == true) {
					AddResponse("волк ещё на левом берегу");
				} else if (man_left == true) {
					AddResponse("крестьянин ещё на левом берегу");
				} else {
					wolf_left = true;
					man_left = true;
					CheckState();
				}
				return;
			}

			if (cabbage_back_regex.IsMatch(p.Text)) {
				if (cabbage_left == true) {
					AddResponse("капуста ещё на левом берегу");
				} else if (man_left == true) {
					AddResponse("крестьянин ещё на левом берегу");
				} else {
					cabbage_left = true;
					man_left = true;
					CheckState();
				}
				return;
			}

			if (goat_regex.IsMatch(p.Text)) {
				if (goat_left == false) {
					AddResponse("коза уже на правом берегу");
				} else if (man_left == false) {
					AddResponse("крестьянин уже на правом берегу");
				} else {
					goat_left = false;
					man_left = false;
					CheckState();
				}
				return;
			}

			if (wolf_regex.IsMatch(p.Text)) {
				if (wolf_left == false) {
					AddResponse("волк уже на правом берегу");
				} else if (man_left == false) {
					AddResponse("крестьянин уже на правом берегу");
				} else {
					wolf_left = false;
					man_left = false;
					CheckState();
				}
				return;
			}

			if (cabbage_regex.IsMatch(p.Text)) {
				if (cabbage_left == false) {
					AddResponse("капуста уже на правом берегу");
				} else if (man_left == false) {
					AddResponse("крестьянин уже на правом берегу");
				} else {
					cabbage_left = false;
					man_left = false;
					CheckState();
				}
				return;
			}

			if (forward_regex.IsMatch(p.Text)) {
				if (man_left == false) {
					AddResponse("крестьянин уже на правом берегу");
				} else {
					man_left = false;
					CheckState();
				}
				return;
			}
		
			if (back_regex.IsMatch(p.Text)) {
				if (man_left == true) {
					AddResponse("крестьянин ещё на левом берегу");
				} else {
					man_left = true;
					CheckState();
				}
				return;
			}
        }
		if (result.Phrases.Length > 0 && result.Phrases[0].Text != "") {
			AddResponse("я тебя не понимаю");
		}*/
    }

}
