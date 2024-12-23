using Banking.Onboarding.Domain.Models;
using System.Xml.Linq;

namespace Banking.Onboarding.Domain.Events;
internal record DocumentoscopyResponseInvalidEvent (string Name, string Email) : OnboardingEvent("DocumentoscopyResponseInvalidEvent", Name, Email);
