namespace GraphQL.Data.Mutations.AddSpeaker
{
    public record AddSpeakerInput(
        string Name,
        string? Bio,
        string? WebSite);
}