using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public Player Player;

    int isNotGroundedCounter;

    void Awake()
    {
        if(Player == null) Debug.LogError(gameObject.name + " : Player Component Missing");
    }

    //tag가 ground일 경우에 체크, 콜라이더가 하나이던 두개이던 상관 없이 체크하기 위한 로직 바닥과 벽을 체크하기 위한 로직.
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            for (int i = 0; i < other.contactCount; i++)
            {
                //땅이 감지되는 경우 isGrounded = true;
                if (other.GetContact(i).normal == Vector2.up)
                {
                    isNotGroundedCounter = 0;
                    Player.SetIsGrounded(true);
                    break;
                }
                //땅이 감지되지 않는 경우가 세프레임 연속 있어야만, isGrounded = false
                else
                {
                    isNotGroundedCounter++;
                    if (isNotGroundedCounter >= 3)
                    {
                        isNotGroundedCounter = 0;
                        Player.SetIsGrounded(false);
                        break;
                    }
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D other) => Player.SetIsGrounded(false);
}
