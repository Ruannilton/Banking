using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.Abstractions;
public interface IOnboardingEventQueue
{
    Task PushEvent<E>(E e) where E: OnboardingEvent;
}
