using Banking.Onboarding.Domain.Models;

namespace Banking.Onboarding.Domain.Events;
internal record DocumentSentToDocumentoscopyEvent(string Name, string Email) : OnboardingEvent("DocumentSentToDocumentoscopyEvent", Name, Email);