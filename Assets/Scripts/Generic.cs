using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    static public class Generic
    {
        static public void LimitYVelocity(float limit, Rigidbody2D rb)
        {
            int gravityMultiplier = (int)(Mathf.Abs(rb.gravityScale) / rb.gravityScale);
            if (rb.velocity.y * -gravityMultiplier > limit)
                rb.velocity = Vector2.up * -limit * gravityMultiplier;
        }
        static public void CreateGamemode(Rigidbody2D rb, PlayerMove host, bool onGroundRequired, float initalVelocity, float gravityScale, bool canHold = false, bool flipOnClick = false, float rotationMod = 0, float yVelocityLimit = Mathf.Infinity)
        {
            if (!Input.GetKey(KeyCode.Space)|| canHold && host.OnGround())
                host.clickProcessed = false;

            rb.gravityScale = gravityScale * host.Gravity;
            LimitYVelocity(yVelocityLimit, rb);

            if (Input.GetKey(KeyCode.Space))
            {
                if (host.OnGround() && !host.clickProcessed || !onGroundRequired && !host.clickProcessed)
                {
                    host.clickProcessed = true;
                    rb.velocity = Vector2.up * initalVelocity * host.Gravity;
                    host.Gravity *= flipOnClick ? -1 : 1;
                }
            }

            if (host.OnGround() || !onGroundRequired)
                host.Sprite.rotation = Quaternion.Euler(0, 0, Mathf.Round(host.Sprite.rotation.eulerAngles.z / 90) * 90);
            else
                host.Sprite.Rotate(Vector3.back, rotationMod * Time.deltaTime * host.Gravity);
        }
    }
}
