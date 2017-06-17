// 代码编写：郭进明  |  技术分享博客：http://www.cnblogs.com/GJM6/ 

using UnityEngine;
using System.Collections;

namespace GJM
{
    /// <summary>
    /// 输入控制类
    /// </summary>  
    public class InputController : MonoBehaviour
    {


        public delegate void JoystickManagerDelegate(Vector2 move);
        public JoystickManagerDelegate joystick;

        #region [ Init ]
        void OnEnable()
        {
            InitClick();

        }
        void OnDisable()
        {
            UnRegistClick();
        }
        void OnDestroy()
        {
            UnRegistClick();
        }
        #endregion
        


        #region EasyTouch 
     
        private void InitClick()
        {
            EasyJoystick.On_JoystickMoveStart += On_JoystickMoveStart;
            EasyJoystick.On_JoystickMove += On_JoystickMove;
            EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
        }
        private void UnRegistClick()
        {
            EasyJoystick.On_JoystickMoveStart -= On_JoystickMoveStart;
            EasyJoystick.On_JoystickMove -= On_JoystickMove;
            EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
        }
           
 
        private void On_JoystickMoveStart(MovingJoystick move)
        {
        
        }
        private void On_JoystickMove(MovingJoystick move)
        {
            if (move.joystickName != "joystick") return;
            float joyPosX = move.joystickAxis.x;
            float joyPosY = move.joystickAxis.y;
            if (joyPosX != 0 || joyPosY != 0 && joystick!=null)
            {
                joystick(new Vector2(joyPosX, joyPosY));
            }
        }
        private void On_JoystickMoveEnd(MovingJoystick move)
        {
            if (move.joystickName != "joystick") return;
            float joyPosX = move.joystickAxis.x;
            float joyPosY = move.joystickAxis.y;
            if (joyPosX != 0 || joyPosY != 0 && joystick != null)
            {
                joystick(new Vector2(joyPosX, joyPosY));
            }             
        }
      

        #endregion
    }
}
