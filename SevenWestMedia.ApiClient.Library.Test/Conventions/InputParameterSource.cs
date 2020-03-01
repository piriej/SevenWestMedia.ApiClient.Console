using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using Fixie;
using Microsoft.Extensions.DependencyInjection;

namespace SevenWestMedia.ApiClient.Library.Test.Conventions
{
    public class InputParameterSource : ParameterSource
    {
        private Fixture _fixture
            ;
        public ServiceProvider Services { get; set; }

        public InputParameterSource(ServiceProvider services)
        {
            Services = services;
            _fixture = new Fixture();
        }

        public IEnumerable<object[]> GetParameters(MethodInfo method)
        {
            return GetParametersFromAttribs(method)
                .Concat(GetParametersFromContainerOrFixture(method));
        }

        private IEnumerable<object[]> GetParametersFromContainerOrFixture(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var objects = new object[parameters.Length];

            for (var index = 0; index < parameters.Length; index++)
            {
                var parameterInfo = parameters[index];
                var service = Services.GetService(parameterInfo.ParameterType);
                if (service == null && !parameterInfo.ParameterType.IsInterface)
                {
                    var context = new SpecimenContext(_fixture);
                    service = context.Resolve(parameterInfo.ParameterType);
                }

                objects[index] = service;
            }

            yield return objects;
        }

        private IEnumerable<object[]> GetParametersFromAttribs(MethodInfo method)
        {
            var inputAttributes = method.GetCustomAttributes<Input>().ToArray();

            foreach (var input in inputAttributes)
                yield return input.Parameters;
        }
    }
}