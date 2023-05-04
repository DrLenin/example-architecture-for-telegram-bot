namespace PC.Api.Apis;

public class PostCardApi : IApi
{
    public void RegisterActions(WebApplication application)
    {
        application.MapPost("81b93a69-686c-472d-ad34-ba3a5e377afa", HandleMessage);
    }

    private async Task<IResult> HandleMessage([FromBody]object update, ICommandService commandService,
        IMapper mapper, CancellationToken cancellationToken)
    {
        var deserializeUpdate = JsonConvert.DeserializeObject<Update>(update.ToString()!);
        var commandRequest = mapper.Map<CommandRequest>(deserializeUpdate);
        
        await commandService.Execute(commandRequest, cancellationToken);

        return await Task.FromResult(Results.Ok("HandleMessage"));
    }
}