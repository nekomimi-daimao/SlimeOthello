using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Slime
{
    [RequireComponent(typeof(Animator))]
    public class SlimeAnimation : MonoBehaviour
    {
        [SerializeField]
        private AnimationClip idle;

        [SerializeField]
        private AnimationClip walk;

        [SerializeField]
        private AnimationClip jump;

        private Dictionary<SlimeAct, AnimationClip> _clips;
        private List<SlimeAct> _indexList;

        private PlayableGraph _playableGraph;
        private AnimationMixerPlayable _animationMixerPlayable;

        private void Awake()
        {
            SetupClips();
        }

        internal void Init(Slime slime)
        {
            slime.act.TakeUntilDisable(this).Subscribe(ChangeAnim);
        }

        private void SetupClips()
        {
            _clips = new Dictionary<SlimeAct, AnimationClip>
            {
                { SlimeAct.Idle, idle },
                { SlimeAct.Walk, walk },
                { SlimeAct.Jump, jump },
            };
            _indexList = _clips.Keys.ToList();

            _playableGraph = PlayableGraph.Create();
            this.OnDestroyAsObservable()
                .Subscribe(_ => { _playableGraph.Destroy(); });

            _animationMixerPlayable = AnimationMixerPlayable.Create(_playableGraph, _clips.Count);
            for (var count = 0; count < _indexList.Count; count++)
            {
                var playable = AnimationClipPlayable.Create(_playableGraph, _clips[_indexList[count]]);
                _animationMixerPlayable.ConnectInput(count, playable, 0);
            }

            var output = AnimationPlayableOutput.Create(_playableGraph, "output", GetComponent<Animator>());
            output.SetSourcePlayable(_animationMixerPlayable);
            _playableGraph.Play();
        }

        private void ChangeAnim(SlimeAct act)
        {
            var index = _indexList.IndexOf(act);
            if (index == -1)
            {
                return;
            }

            for (var count = 0; count < _indexList.Count; count++)
            {
                _animationMixerPlayable.SetInputWeight(count, count == index ? 1f : 0f);
            }
        }
    }
}
