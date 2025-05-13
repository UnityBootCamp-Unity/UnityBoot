using UnityEngine;
using UnityEngine.UIElements;

//발사대에 연결할 스크립트
//발사 버튼을 눌렀을 경우 총알 발사
//현재 총알에는 방향에 따라 이동하는 코드가 구현이 되어있음.
//따라서 버튼 누르면 생성되게만 만들어주면 구현 완료

public class PlayerFire : MonoBehaviour
{
    public GameObject bulletFactory; //총알 공장

    public GameObject firePosition;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    
    void Shoot()
    {
        var bullet = Instantiate(bulletFactory);
        bullet.transform.position = firePosition.transform.position;
    }




 
}
