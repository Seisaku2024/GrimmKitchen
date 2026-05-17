using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Threading;
using Cysharp.Threading.Tasks;

public static class AnimatorExtension
{
    public static void SetTriggerOneShot(this Animator self, string name)
    {
        var cts = new CancellationTokenSource();

        self.SetTrigger(name);
        self.OnAnimatorMoveAsObservable()
            .Subscribe(_ => { self.ResetTrigger(name); cts.Cancel(); })
            .AddTo(cts.Token)
            .AddTo(self.GetCancellationTokenOnDestroy());
    }

    public static void SetTriggerOneShot(this Animator self, int id)
    {
        var cts = new CancellationTokenSource();

        self.SetTrigger(id);
        self.OnAnimatorMoveAsObservable()
            .Subscribe(_ => { self.ResetTrigger(id); cts.Cancel(); })
            .AddTo(cts.Token)
            .AddTo(self.GetCancellationTokenOnDestroy());
    }
}

