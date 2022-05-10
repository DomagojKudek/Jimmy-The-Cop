
using UnityEngine;

[CreateAssetMenu(fileName = "New Unlocked Levels", menuName = "Unlocked Levels")]
public class UnlockedLevels : ScriptableObject
{
    /*
	private static UnlockedLevels _inst;

	public static UnlockedLevels Instance{
		get{
			if(_inst == null){
				_inst = ScriptableObject.CreateInstance<UnlockedLevels>();
			}
			return _inst;
		}
	}
	*/
    //binarni array ako je vrijednost 1 onda je index broj
    // area koja je enableana u lvl selctu
    //npr unlockedLevels[1] = 1 - lvl select area 1 je enabled
    public int[] unlockedLevels;
}