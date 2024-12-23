namespace Banking.Onboarding.Domain.Models;

public record OnboardingEvent(string EventName, string CustomerName, string CustomerEmail);