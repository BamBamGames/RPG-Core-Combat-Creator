using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.PlayableAnimation
{
    public class PlayableAnimator : MonoBehaviour
    {
        public AnimationClip Idle;
        public AnimationClip Walk;
        public AnimationClip Run;
        private const float walkSpeed = 1.558401f;
        private const float runSpeed = 5.662317f;

        public AnimationClip Death;
        public AnimationClip UnarmedAttack;

        private PlayableGraph playableGraph;
        private AnimationPlayableOutput playableOutput;
        private AnimationMixerPlayable defaultMixer;
        private AnimationMixerPlayable playableLocomotion;
        private AnimationClipPlayable playableDeath;
        private AnimationClipPlayable playableUnarmedAttack;

        private void Start()
        {
            InitPlayableAnimatorController();
        }

        private void OnDisable()
        {
            playableGraph.Stop();
        }

        private void OnDestroy()
        {
            playableGraph.Destroy();
        }

        private void InitPlayableAnimatorController()
        {
            playableGraph = PlayableGraph.Create("PlayableAnimator");
            playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            GraphVisualizerClient.Show(playableGraph);

            // blend的walk动画
            var idlePlayable = AnimationClipPlayable.Create(playableGraph, Idle);
            var walkPlayable = AnimationClipPlayable.Create(playableGraph, Walk);
            var runPlayable = AnimationClipPlayable.Create(playableGraph, Run);
            playableLocomotion = AnimationMixerPlayable.Create(playableGraph, 3);
            playableGraph.Connect(idlePlayable, 0, playableLocomotion, 0);
            playableGraph.Connect(walkPlayable, 0, playableLocomotion, 1);
            playableGraph.Connect(runPlayable, 0, playableLocomotion, 2);

            // death
            playableDeath = AnimationClipPlayable.Create(playableGraph, Death);

            // attack
            playableUnarmedAttack = AnimationClipPlayable.Create(playableGraph, UnarmedAttack);

            // 创建一个Output节点，类型是Animation，名字是Animation，目标对象是物体上的Animator组件
            defaultMixer = AnimationMixerPlayable.Create(playableGraph, 2);
            playableOutput = AnimationPlayableOutput.Create(playableGraph, "AnimationOutput", GetComponent<Animator>());
            playableOutput.SetSourcePlayable(defaultMixer);

            // 播放这个graph
            playableGraph.Play();
            PlayWalk(0);
        }

        private void BlendToPlayable(Playable playable)
        {
            var inputCount = defaultMixer.GetInputCount();
            for (var i = 0; i < inputCount; i++)
            {
                if (defaultMixer.GetInput(i).Equals(playable))
                {
                    return;
                }
            }

            // 直接把旧的playable拼接到新的mixer中
            var newDefaultMixer = AnimationMixerPlayable.Create(playableGraph, 2);
            playableOutput.SetSourcePlayable(newDefaultMixer);
            playableGraph.Connect(defaultMixer, 0, newDefaultMixer, 0);
            playableGraph.Connect(playable, 0, newDefaultMixer, 1);

            defaultMixer = newDefaultMixer;
            StartCoroutine(BlendMixer(defaultMixer, 0.5f));

            // 如果有两个或者更多，则先把这些保存为一个mixer，把mixer与新的playable做blend

            // - 动画本身的 blend：locomotion（idle、walk、run）用速度做blend动画的权重
            // - 切换动画的 blend：从locomotion切换到attack，以及从attack切换回到locomotion时是需要有个过度的
            // - 动画切换的条件有指定参数的，也有自动退出回来的
        }
        
        private IEnumerator BlendToPlayable(Playable playable, float autoExitTime)
        {
            yield return new WaitForSeconds(autoExitTime); 
            BlendToPlayable(playable);
        }

        private IEnumerator BlendMixer(AnimationMixerPlayable mixerPlayable, float totalBlendTime)
        {
            var blendTime = 0f;
            while (blendTime < totalBlendTime)
            {
                var newWeight = blendTime / totalBlendTime;
                mixerPlayable.SetInputWeight(0, 1 - newWeight);
                mixerPlayable.SetInputWeight(1, newWeight);
                blendTime += Time.deltaTime;
                yield return null;
            }

            mixerPlayable.DisconnectInput(0);
        }

        public void PlayWalk(float speed)
        {
            BlendToPlayable(playableLocomotion);

            speed = Mathf.Max(0, speed);
            if (speed < walkSpeed)
            {
                var walkInputWeight = speed / walkSpeed;
                playableLocomotion.SetInputWeight(0, 1 - walkInputWeight);
                playableLocomotion.SetInputWeight(1, walkInputWeight);
                playableLocomotion.SetInputWeight(2, 0);
            }
            else
            {
                var runInputWeight = (speed - walkSpeed) / runSpeed;
                playableLocomotion.SetInputWeight(0, 0);
                playableLocomotion.SetInputWeight(1, 1 - runInputWeight);
                playableLocomotion.SetInputWeight(2, runInputWeight);
            }
        }

        public void PlayDeath()
        {
            BlendToPlayable(playableDeath);
        }

        public void PlayAttack()
        {
            BlendToPlayable(playableUnarmedAttack);
            // 攻击状态自动切换回locomotion
            // StartCoroutine(BlendToPlayable(playableLocomotion, 0.5f + 0.7051108f));
        }
    }
}