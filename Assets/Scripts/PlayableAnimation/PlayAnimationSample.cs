using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace RPG.PlayableAnimation
{
    // 需要同物体上有Animator组件，但是不需要给他赋值Animator Controller
    [RequireComponent(typeof(Animator))]
    public class PlayAnimationSample : MonoBehaviour
    {
        public AnimationClip clip;
        private PlayableGraph playableGraph;

        private void Start()
        {
            // 创建一个PlayableGraph并给它命名
            playableGraph = PlayableGraph.Create("PlayAnimationSample");
            // 创建一个Output节点，类型是Animation，名字是Animation，目标对象是物体上的Animator组件
            var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
            // 创建一个动画剪辑Playable，将clip传入进去
            var clipPlayable = AnimationClipPlayable.Create(playableGraph, clip);
            // 将playable连接到output
            playableOutput.SetSourcePlayable(clipPlayable);
            // 播放这个graph
            playableGraph.Play();
        }

        private void OnDisable()
        {
            // 销毁所有的Playables和PlayableOutputs
            playableGraph.Destroy();
        }
    }
}