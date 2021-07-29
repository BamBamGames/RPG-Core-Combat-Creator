using UnityEngine;

namespace RPG.PlayableAnimation
{
    /// <summary>
    /// 完整的关于IK的入门教程见 https://zhuanlan.zhihu.com/p/37995835
    /// </summary>
    public class LookAtIK : MonoBehaviour
    {
        public Transform lookAtTarget;
	
        private Animator _animator;
        void Start()
        {
            _animator = this.GetComponent<Animator>();
        }

        // Unity 2019 关于这个函数默认情况下是不会被调用的
        // 有一个小坑，具体见： http://answers.unity.com/answers/1749283/view.html
        void OnAnimatorIK(int layerIndex)
        {
            if(_animator != null)
            {
                //仅仅是头部跟着变动
                _animator.SetLookAtWeight(1);
                //身体也会跟着转, 弧度变动更大
                // _animator.SetLookAtWeight(1, 1, 1, 1);
                if(lookAtTarget != null)
                {
                    _animator.SetLookAtPosition(lookAtTarget.position);
                }
            }
        }
    }
}