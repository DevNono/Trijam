using UnityEngine;

public class Day : MonoBehaviour
{
    public int dayNumber = 1;
    public int remainingPNJCountOfTheDay = 0;
    public Queue queue;
    [SerializeField]
    private GameObject pnjPrefab;
    [SerializeField]
    private Transform pnjQueue;

    void StartPNJ() {
        PNJ pnj = queue.PopPNJ();
        queue.ChooseDialog(pnj);
        // Instanciate dialog, spawn last pnj
        GameObject instance = Instantiate(pnjPrefab, pnjQueue);
        instance.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = pnj.sprite;
        instance.GetComponent<Animator>().SetInteger("position", Queue.QUEUE_MAX_SIZE);

        // Destroy old prefab and move all prefabs to their next position
        Destroy(pnjQueue.GetChild(0).gameObject);
        for (int i = 0; i < pnjQueue.childCount; i++) {
            Animator animator = pnjQueue.GetChild(i).GetComponent<Animator>();
            animator.SetInteger("position", animator.GetInteger("position") - 1);
        }

        // Spawn dialog with proper data
        // and wait for decision
    }
}
