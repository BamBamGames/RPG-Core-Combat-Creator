using System;
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
        
        public AnimationClip Death;
        public AnimationClip UnarmedAttack;
        
        private PlayableGraph playableGraph;
        private AnimationPlayableOutput playableOutput;
        private AnimationMixerPlayable playableBlendWalk;

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

            // 创建一个Output节点，类型是Animation，名字是Animation，目标对象是物体上的Animator组件
            playableOutput = AnimationPlayableOutput.Create(playableGraph, "AnimationOutput", GetComponent<Animator>());
            
            // blend的walk动画
            var idlePlayable = AnimationClipPlayable.Create(playableGraph, Idle);
            var walkPlayable = AnimationClipPlayable.Create(playableGraph, Walk);
            // var runPlayable = AnimationClipPlayable.Create(playableGraph, Run);
            playableBlendWalk = AnimationMixerPlayable.Create(playableGraph, 3);
            playableGraph.Connect(idlePlayable, 0, playableBlendWalk, 0);
            playableGraph.Connect(walkPlayable, 0, playableBlendWalk, 1);
            // playableGraph.Connect(runPlayable, 0, playableMixer, 2);
            playableBlendWalk.SetInputWeight(0, 1);
            playableBlendWalk.SetInputWeight(1, 0);

            playableOutput.SetSourcePlayable(playableBlendWalk);
            
            // 播放这个graph
            playableGraph.Play();
        }

        public void PlayWalk(float speed)
        {
            
        }

        public void PlayDeath()
        {
            
        }

        public void PlayAttack()
        {
            
        }
    }
}