namespace MauiMemoryGame.Base;

public interface IAnimatedPage
{
    Task RunDisappearingAnimationAsync();
    Task RunAppearingAnimationAsync();
}
