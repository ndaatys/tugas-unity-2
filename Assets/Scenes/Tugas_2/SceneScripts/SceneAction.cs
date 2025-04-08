using UnityEngine;

namespace Tugas_2.SceneScripts
{
    public class SceneAction : MonoBehaviour
    {
        [Header("Attribute")]
        [SerializeField] Transform square;
        [SerializeField] Transform circle;
        [SerializeField] Transform triangle;

        bool circleReverse = false;
        bool circleStop = false;

        void Update()
        {
            var delta = Time.deltaTime;

            if (square)
            {
                var speed = 1;
                square.localPosition += Vector3.right * speed * delta;
            }
            
            if (circle && !circleStop)
            {
                var speed = 3;
                if (circleReverse)
                {
                    speed *= -1;
                }
                circle.localPosition += Vector3.right * speed * delta;
                
                if (circle.localPosition.x >= 10)
                {
                    circleReverse = true;
                }
                if (circle.localPosition.x <= -10 && circleReverse)
                {
                    circleReverse = false;
                    circleStop = true;
                    var pos = circle.localPosition;
                    pos.x = -10;
                    circle.localPosition = pos;
                }
            }

            if (triangle)
            {
                var speed = 2;
                if (triangle.localPosition.x < 10)
                {
                    triangle.localPosition += Vector3.right * speed * delta;
                    if (triangle.localPosition.x > 10)
                    {
                        var currentPos = triangle.localPosition;
                        currentPos.x = 10;
                        triangle.localPosition = currentPos;
                    }
                }
            }
        }
    }
}