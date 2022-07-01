using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using Practica.Application.UseCase.V1.Pedidos.Command;
using Practica.Application.UseCase.V1.Pedidos.Queries;
using Practica.Domain.Entities;

namespace Practica.API.Controllers.V1
{
    public static class MinimalApiEndpointsV1
    {
        public static void PracticaControllerV1(this WebApplication app)
        {

            app.MapPost("v1/api/pedido", async ([FromBody] PedidoCreateRequest requestData, [FromServices] IMediator mediator, HttpResponse response) =>
            {
                if (!MiniValidator.TryValidate(requestData, out var errors))
                    return Results.ValidationProblem(errors);

                var item = await mediator.Send(new CreatePedidoCommand() { Pedido = requestData});
                return Results.Created($"/api/{item.Id}", item);
            })
            .WithName("Generar Pedido").ProducesValidationProblem(StatusCodes.Status400BadRequest).Produces(StatusCodes.Status201Created);

            app.MapGet("v1/api/pedido/{id}", async ([FromHeaderAttribute] Guid id, [FromServices] IMediator mediator) =>
            {
                var pedido = await mediator.Send(new QueryGetByIdPedido() { Id = id});
                if (pedido == null) return Results.NotFound();

                return Results.Ok(pedido);

            }).WithName("Obtener pedido").Produces<Pedido>(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status404NotFound);

            app.MapGet("v1/api/pedido", async ([FromServices] IMediator mediator) =>
            {
                var pedidos = await mediator.Send(new QueryGetAllPedidos());
                
                return Results.Ok(pedidos);

            }).WithName("Obtener todos los pedidos").Produces<IEnumerable<Pedido>>(StatusCodes.Status200OK);

            app.MapPut("v1/api/pedido/{id}", async ([FromHeaderAttribute] Guid id, [FromBody] PedidoUpdateRequest requestData, [FromServices] IMediator mediator) =>
            {
                try
                {
                    if (!MiniValidator.TryValidate(requestData, out var errors))
                        return Results.ValidationProblem(errors);

                    var pedido = await mediator.Send(new UpdatePedidoCommand() { Id = id, NumeroDePedido = requestData.NumeroDePedido, Estado = "ASIGNADO" });

                    return Results.Ok(pedido);
                }
                catch (ArgumentNullException e)
                {
                    return Results.NotFound(e);
                }
            }).WithName("Actualizar numero de envío").Produces<Pedido>(StatusCodes.Status200OK).ProducesValidationProblem(StatusCodes.Status400BadRequest).ProducesProblem(StatusCodes.Status404NotFound);

        }

    }
}
