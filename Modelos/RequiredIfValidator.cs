using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Modelos
{
    public class CotizacionRequiredIfValidator : DataAnnotationsModelValidator<CotizacionRequiredIfAttribute>
    {
        public CotizacionRequiredIfValidator(ModelMetadata metadata, ControllerContext context, CotizacionRequiredIfAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            // get a reference to the property this validation depends upon
            var field = Metadata.ContainerType.GetProperty(Attribute.DependentProperty);

            if (field != null)
            {
                // get the value of the dependent property
                var value = field.GetValue(container, null);

                // compare the value against the target value
                if ((value == null && Attribute.TargetValue == null) ||
                    (!value.Equals(Attribute.TargetValue)))
                {
                    // match => means we should try validating this field
                    if (!Attribute.IsValid(Metadata.Model))
                        // validation failed - return an error
                        yield return new ModelValidationResult { Message = ErrorMessage };
                }
            }
        }
    }
}