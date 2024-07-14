using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MornScreenFade
{
    public sealed class MornScreenFadeImage : MonoBehaviour, IMornScreenFade
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _defaultFillColor = Color.white;
        [SerializeField] private float _defaultDuration = 0.3f;
        private CancellationTokenSource _cts;

        void IMornScreenFade.FadeFillImmediate()
        {
            FadeFillAsync(0, _defaultFillColor).Forget();
        }

        void IMornScreenFade.FadeFill()
        {
            FadeFillAsync(_defaultDuration, _defaultFillColor).Forget();
        }

        void IMornScreenFade.FadeFill(float duration)
        {
            FadeFillAsync(duration, _defaultFillColor).Forget();
        }

        async UniTask IMornScreenFade.FadeFillAsync(CancellationToken ct)
        {
            await FadeFillAsync(_defaultDuration, _defaultFillColor, ct);
        }

        UniTask IMornScreenFade.FadeFillAsync(float duration, CancellationToken ct)
        {
            return FadeFillAsync(duration, _defaultFillColor, ct);
        }

        void IMornScreenFade.FadeClearImmediate()
        {
            FadeClearAsync(0).Forget();
        }

        void IMornScreenFade.FadeClear()
        {
            FadeClearAsync(_defaultDuration).Forget();
        }

        public async UniTask FadeClearAsync(CancellationToken ct = default)
        {
            await FadeClearAsync(_defaultDuration, ct);
        }
        public async UniTask FadeClearAsync(float duration, CancellationToken ct = default)
        {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            _image.raycastTarget = false;
            var endColor = _image.color;
            endColor.a = 0;
            await ColorTweenTask(_image, _image.color, endColor, duration, _cts.Token);
        }

        public async UniTask FadeFillAsync(float duration, Color fillColor, CancellationToken ct = default)
        {
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            _image.raycastTarget = true;
            await ColorTweenTask(_image, _image.color, fillColor, duration, _cts.Token);
        }

        private async static UniTask ColorTweenTask(Image image, Color startColor, Color endColor, float duration, CancellationToken ct)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                image.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                await UniTask.Yield(ct);
            }

            image.color = endColor;
        }
    }
}