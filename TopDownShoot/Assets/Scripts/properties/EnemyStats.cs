using UnityEngine;

[CreateAssetMenu(menuName ="Enemy")]
public class EnemyStats :ScriptableObject
{
    public int HP;
    public int Damage;
    public int Speed;
    public int Points; //количество очков за убийство
    public int SpawnWeight; //шанс спавна
}
