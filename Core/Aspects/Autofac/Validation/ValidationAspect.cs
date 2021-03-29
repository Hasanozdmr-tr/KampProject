
using Castle.DynamicProxy;
using Core.CrossCuttingConcern.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {    // Aspect demek Başında sonunda çalışacak metod

        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil");
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation) //invocation lar bizim operasyonlarımız yani Add metodu
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            //productvalidator ı newledi burda. Program çalışırken newlesin istiyorsan activator kullanırsın.
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            //validatorType ın base type ı AbstractValidator dır. Bunun Generic argument [0]ı bulunur. Yani product
            // entityType Product ın tipi olur yani.
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            //metodun(invocation) argumanlarını gez.Tipi entitytype a eşitse yani product a eşitle.(GetType burda 1den fazla olabilir.)
            foreach (var entity in entities)   //entityler arasında gez ve validate et.
            {
                ValidationTool.Validate(validator, entity);
            }
            // Yukarıyı tek tek incele.. OK.
        }
    }
}
