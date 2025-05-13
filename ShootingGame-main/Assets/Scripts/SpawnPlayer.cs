using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject[] character_list;
    private GameObject character_player;
    public Transform spawnPoint;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        int current_character = PlayerCharacter.Instance.Character;
        character_player = Instantiate(character_list[current_character], spawnPoint.position, spawnPoint.rotation);
    }
}
