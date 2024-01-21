using UnityEngine;

public class Day : MonoBehaviour
{
    public int dayNumber = 1;
    public int dayLength = 0;
    public int remainingPNJCountOfTheDay = 0;
    public int kickCount = 0;
    public Queue queue;
    [SerializeField]
    private GameObject pnjPrefab;
    [SerializeField]
    private Transform pnjQueue;
    private bool firstDay = true;

    public int resource_money = 50;
    public int resource_fidelity = 50;
    public int resource_badness = 50;
    public int resource_divine_authority = 50;

    [SerializeField]
    private AnimatedProgressBar money_bar;
    [SerializeField]
    private AnimatedProgressBar fidelity_bar;
    [SerializeField]
    private AnimatedProgressBar badness_bar;
    [SerializeField]
    private AnimatedProgressBar divine_auth_bar;
    [SerializeField]
    private UpdateText day_counter;
    [SerializeField]
    private UpdateText kick_counter;
    [SerializeField]
    private AnimatedProgressBar day_progress;
    [SerializeField]
    private Dialog noMoneyDialog;
    [SerializeField]
    private PNJ noMoneyPNJ;
    [SerializeField]
    private Dialog noFidelityDialog;
    [SerializeField]
    private PNJ noFidelityPNJ;
    [SerializeField]
    private Dialog noBadnessDialog;
    [SerializeField]
    private PNJ noBadnessPNJ;
    [SerializeField]
    private Dialog noDivineAuthorityDialog;
    [SerializeField]
    private PNJ noDivineAuthorityPNJ;

    [SerializeField]
    private AnimatedText dialogText;

    private Dialog currentDialog;
    private Dialog exclusiveDialog;
    private PNJ exclusivePNJ;

    public void Start() {
        queue.Initialize();
        for (int i = 0; i < queue.pnjs.Count; i++) {
            GameObject instance = Instantiate(pnjPrefab, pnjQueue);
            instance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = queue.pnjs[i].sprite;
            instance.GetComponent<Animator>().SetInteger("position", i + 1);
        }
        StartPNJ(true);
        money_bar.SetProgress(resource_money / 100f);
        fidelity_bar.SetProgress(resource_fidelity / 100f);
        badness_bar.SetProgress(resource_badness / 100f);
        divine_auth_bar.SetProgress(resource_divine_authority / 100f);
    }

    public void StartPNJ(bool skipDeletion = false) {
        if (exclusiveDialog != null && exclusivePNJ != null) {
            Destroy(pnjQueue.GetChild(0).gameObject);
            GameObject exclusiveInstance = Instantiate(pnjPrefab, pnjQueue);
            exclusiveInstance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = exclusivePNJ.sprite;
            exclusiveInstance.GetComponent<Animator>().SetInteger("position", 0);
            exclusiveInstance.GetComponent<Animator>().SetTrigger("move");
            // Display dialog
            dialogText.StartTextAnimation(exclusiveDialog.text);
            return;
        }

        if (remainingPNJCountOfTheDay < 1) {
            // Jump to next day
            if (firstDay == true) {
                firstDay = false;
                dayNumber = 1;
            } else {
                ++dayNumber;
            }
            day_counter.UpdateTextValue($"Day {dayNumber}");
            dayLength = remainingPNJCountOfTheDay = Random.Range(15, 21);
            kickCount += Mathf.CeilToInt(Random.Range(.2f, .5f) * resource_badness / 20f);
            kickCount = Mathf.Min(9, kickCount);
            kick_counter.UpdateTextValue($"{kickCount}");
        }
        remainingPNJCountOfTheDay--;
        day_progress.SetProgress(1f - (float)remainingPNJCountOfTheDay / dayLength);

        PNJ pnj = queue.PopPNJ();
        currentDialog = queue.ChooseDialog(pnj);
        // Instanciate dialog, spawn last pnj
        GameObject instance = Instantiate(pnjPrefab, pnjQueue);
        instance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = queue.pnjs[^1].sprite;
        instance.GetComponent<Animator>().SetInteger("position", Queue.QUEUE_MAX_SIZE + 1);

        // Destroy old prefab and move all prefabs to their next position
        if (!skipDeletion) Destroy(pnjQueue.GetChild(0).gameObject);
        for (int i = 0; i < pnjQueue.childCount; i++) {
            Animator animator = pnjQueue.GetChild(i).GetComponent<Animator>();
            animator.SetInteger("position", animator.GetInteger("position") - 1);
            animator.GetComponent<Animator>().SetTrigger("move");
        }

        // Spawn dialog with proper data
        // and wait for decision
        dialogText.StartTextAnimation(currentDialog.text);
    }

    public void EndPNJ(int answerIndex) {
        if (currentDialog.dialogType != DialogType.INFORMATIVE) {
            foreach (var resourceCost in currentDialog.resourcesCosts) {
                int value = answerIndex < 1 ? resourceCost.values.Option1 : resourceCost.values.Option2;
                switch (resourceCost.key) {
                     case ResourceType.MONEY:
                        resource_money += value;
                        resource_money = Mathf.Max(0, Mathf.Min(resource_money, 100));
                        money_bar.SetProgress(resource_money / 100f);
                        break;
                     case ResourceType.FIDELITY:
                        resource_fidelity += value;
                        resource_fidelity = Mathf.Max(0, Mathf.Min(resource_fidelity, 100));
                        fidelity_bar.SetProgress(resource_fidelity / 100f);
                        break;
                     case ResourceType.BADNESS:
                        resource_badness += value;
                        resource_badness = Mathf.Max(0, Mathf.Min(resource_badness, 100));
                        badness_bar.SetProgress(resource_badness / 100f);
                        break;
                     case ResourceType.DIVINE_AUTHORITY:
                        resource_divine_authority += value;
                        resource_divine_authority = Mathf.Max(0, Mathf.Min(resource_divine_authority, 100));
                        divine_auth_bar.SetProgress(resource_divine_authority / 100f);
                        break;
                }
            }
        }
        // Check the player is alive
        if (resource_money == 0) {
            exclusiveDialog = noMoneyDialog;
            exclusivePNJ = noMoneyPNJ;
        } else if ((resource_fidelity % 100) == 0) {
            exclusiveDialog = noFidelityDialog;
            exclusivePNJ = noFidelityPNJ;
        } else if (resource_badness == 0) {
            exclusiveDialog = noBadnessDialog;
            exclusivePNJ = noBadnessPNJ;
        } else if ((resource_divine_authority % 100) == 0) {
            exclusiveDialog = noDivineAuthorityDialog;
            exclusivePNJ = noDivineAuthorityPNJ;
        }
        // Jump to next PNJ
        StartPNJ();
    }

    public bool KickPNJ(GameObject obj) {
        if (!CanKick()) return false;
        kickCount--;
        kick_counter.UpdateTextValue($"{kickCount}");
        int position = -1;
        for (int i = 0; i < queue.transform.childCount; i++) {
            if (queue.transform.GetChild(i) == obj.transform) {
                position = i;
                break;
            }
        }
        queue.Kick(position);
        Destroy(queue.transform.GetChild(position).gameObject);
        for (int i = position; i < queue.transform.childCount; i++) {
            Animator anim = queue.transform.GetChild(i).GetComponent<Animator>();
            anim.SetInteger("position", anim.GetInteger("position") - 1);
            anim.SetTrigger("move");
        }
        GameObject instance = Instantiate(pnjPrefab, pnjQueue);
        instance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = queue.pnjs[^1].sprite;
        instance.GetComponent<Animator>().SetInteger("position", Queue.QUEUE_MAX_SIZE);
        return true;
    }

    public bool CanKick() {
        return kickCount > 0;
    }
}
