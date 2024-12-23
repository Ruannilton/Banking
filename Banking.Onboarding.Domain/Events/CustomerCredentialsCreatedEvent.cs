using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.Events;
internal record CustomerCredentialsCreatedEvent(string Name, string Email) : OnboardingEvent("CustomerCredentialsCreatedEvent", Name, Email);
