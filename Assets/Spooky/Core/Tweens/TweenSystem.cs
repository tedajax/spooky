using System.Collections.Generic;
using UnityEngine;

public class TweenSystem
{
    List<Tween> tweens;

    public TweenSystem()
    {
        tweens = new List<Tween>(512);
    }

    public Tween CreateTween(TweenData tweenData)
    {
        var tween = new Tween(tweenData);
        tween.tweenSystem = this;
        tweens.Add(tween);
        return tween;
    }

    public void Update(float dt)
    {
        // Update internal timescales of tweens and call Play/Pause events
        foreach (var tween in tweens) {
            if (!Mathf.Approximately(tween.timescale, tween.internalTimescale)) {
                tween.SetTimescale(tween.timescale);
            }
        }

        // Tick tweens
        foreach (var tween in tweens) {
            float dirScalar = (tween.direction == TweenDirection.Forward) ? 1f : -1f;
            float delta = tween.internalTimescale * dt * dirScalar;

            tween.currentTime += delta;

            bool finished = false;
			bool manuallyStopped = false;

			if (tween.direction == TweenDirection.Forward) {
                finished = tween.currentTime >= tween.duration;
            }
            else {
                finished = tween.currentTime <= 0f;
            }

            finished &= tween.internalTimescale > 0f;

			if (!finished && tween.stopAfterSeconds > 0f)
			{
				tween.stopAfterSeconds -= delta;
				if (tween.stopAfterSeconds <= 0f)
				{
					finished = true;
					manuallyStopped = true;
					tween.stopAfterSeconds = 0f;
				}
			}

			if (finished) {
                bool overrideLooping = (tween.loopBehavior == TweenLoopBehavior.Clamp) || manuallyStopped;
                bool doOnLoop = false;

                if (tween.loopsRemaining > 0 || overrideLooping) {
                    --tween.loopsRemaining;

                    if (tween.loopsRemaining <= 0 || overrideLooping) {
                        tween.SetTimescale(0f);

                        switch (tween.loopBehavior) {
                            case TweenLoopBehavior.Normal:
                            case TweenLoopBehavior.PingPong:
                                tween.currentTime = tween.StartTime;
                                break;

                            case TweenLoopBehavior.Clamp:
                                tween.currentTime = tween.EndTime;
                                break;
                        }

                        tween.OnFinish();
                        return;
                    }
                    else {
                        doOnLoop = true;
                    }
                }
                else {
                    doOnLoop = true;
                }

                tween.currentTime -= tween.duration * dirScalar;

                if (doOnLoop) {
                    tween.OnLoop();
                }
            }
        }
    }
}
