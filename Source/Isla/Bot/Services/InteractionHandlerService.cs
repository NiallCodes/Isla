using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NiallCodes.Launchpad.Hosting.Utilities.Services;

namespace Isla.Bot.Services;

/// <summary>
/// Handles incoming interaction events.
/// </summary>
public class InteractionHandlerService : HostedService
{
    private readonly IServiceProvider _services;
    private readonly DiscordSocketClient _discord;
    private readonly InteractionService _interaction;
    private readonly ILogger<InteractionHandlerService> _logger;

    public InteractionHandlerService(IServiceProvider services)
    {
        _services = services;
        _discord = services.GetRequiredService<DiscordSocketClient>();
        _interaction = services.GetRequiredService<InteractionService>();
        _logger = services.GetRequiredService<ILogger<InteractionHandlerService>>();

        _discord.SlashCommandExecuted += HandleInteractionRequest;
        _discord.UserCommandExecuted += HandleInteractionRequest;
        _discord.MessageCommandExecuted += HandleInteractionRequest;
        _interaction.SlashCommandExecuted += HandleInteractionResult;
        _interaction.ContextCommandExecuted += HandleInteractionResult;
    }

    /// <summary>
    /// Forwards the incoming interaction to the interaction service.
    /// </summary>
    private async Task HandleInteractionRequest(SocketInteraction interaction)
    {
        try
        {
            await _interaction.ExecuteCommandAsync(new SocketInteractionContext(_discord, interaction), _services);
        }
        catch (Exception error)
        {
            _logger.LogError(error, "An exception occurred whilst handling a interaction");
        }
    }

    /// <summary>
    /// Deals with the response of an interaction request.
    /// </summary>
    private async Task HandleInteractionResult(IApplicationCommandInfo _, IInteractionContext ctx, IResult result)
    {
        if (result.IsSuccess)
            return;

        var responseText = result.Error switch
        {
            InteractionCommandError.UnknownCommand => "Sorry, I don't know anything about the command you just used!",
            InteractionCommandError.ConvertFailed => "Invalid arguments!",
            InteractionCommandError.BadArgs => "Invalid arguments!",
            InteractionCommandError.ParseFailed => "Invalid arguments!",
            InteractionCommandError.UnmetPrecondition => result.ErrorReason,
            InteractionCommandError.Unsuccessful => "Sorry, something went wrong! Please try again later.",
            InteractionCommandError.Exception => "Sorry, something went wrong! Please try again later.",
            _ => "Sorry, something went wrong! Please try again later."
        };

        try
        {
            await ctx.Interaction.RespondAsync(responseText);
        }
        catch
        {
            await ctx.Interaction.FollowupAsync(responseText);
        }
    }
}