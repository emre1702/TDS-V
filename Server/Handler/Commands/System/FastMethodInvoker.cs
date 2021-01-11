﻿using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using TDS.Server.Data.Models;

namespace TDS.Server.Handler.Commands.System
{
    internal class FastMethodInvoker
    {
        internal FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            var dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object), typeof(object[]) }, methodInfo.DeclaringType!.Module);
            var ilGenerator = dynamicMethod.GetILGenerator();

            // Get the types of each parameter
            var paramTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();

            // Load the object
            ilGenerator.Emit(OpCodes.Ldarg_0);

            // Load all arguments onto the stack.
            for (int i = 0; i < paramTypes.Length; i++)
            {
                // Load the argument from the array. 
                ilGenerator.Emit(OpCodes.Ldarg_1);
                EmitInt(i, ilGenerator);
                ilGenerator.Emit(OpCodes.Ldelem_Ref);
                EmitCast(paramTypes[i], ilGenerator);
            }

            // Call the method
            ilGenerator.EmitCall(OpCodes.Call, methodInfo, null);

            // Return the value
            if (methodInfo.ReturnType == typeof(void)) ilGenerator.Emit(OpCodes.Ldnull);
            else if (methodInfo.ReturnType.IsValueType) ilGenerator.Emit(OpCodes.Box, methodInfo.ReturnType);
            ilGenerator.Emit(OpCodes.Ret);

            return (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
        }

        static void EmitCast(Type toType, ILGenerator gen)
        {
            gen.Emit(toType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, toType);
        }

        static void EmitInt(int i, ILGenerator gen)
        {
            if (i > 8)
            {
                gen.Emit(OpCodes.Ldc_I4_S, (byte)i); // cba to support above 255
            }
            else
            {
                OpCode code = i switch
                {
                    0 => OpCodes.Ldc_I4_0,
                    1 => OpCodes.Ldc_I4_1,
                    2 => OpCodes.Ldc_I4_2,
                    3 => OpCodes.Ldc_I4_3,
                    4 => OpCodes.Ldc_I4_4,
                    5 => OpCodes.Ldc_I4_5,
                    6 => OpCodes.Ldc_I4_6,
                    7 => OpCodes.Ldc_I4_7,
                    8 => OpCodes.Ldc_I4_8,
                    _ => throw new Exception()
                };

                gen.Emit(code);
            }
        }
    }
}
