using FluentAssertions;
using FoodStore.Api.Controllers;
using FoodStore.Data.Entities;
using FoodStore.Service.Abstracts;
using FoodStore.Service.Authorization;
using FoodStore.Service.Context;
using FoodStore.Service.Implementations;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace FoodStore.XUnitTest;

public class CategoryControllerTests
{
    
    

    [Fact]
    public async Task  DeleteRoleAsync_WhenRoleIsDeleted_Retune_Succeeded()
    {
        //Arrange
         var roleId = "1";
        var role = new IdentityRole{
           Id = "kjasn"
        };
        var mockRole = new Mock<RoleManager<IdentityRole>>(MockBehavior.Strict,null, null, null, null, null, null, null, null);
       

        mockRole.Setup(x => x.FindByIdAsync(roleId)).ReturnsAsync(role);
        mockRole.Setup(m => m.DeleteAsync(role)).ReturnsAsync(IdentityResult.Success);

        var authorizationService = new AuthorizationService(mockRole.Object,null);

       
       
        //Act

        var result = await authorizationService.DeleteRoleAsync(roleId);

        //Assert

        // result.Should().Be("Succeeded");
        Assert.Equal("Succeeded", result);

    

    }
}