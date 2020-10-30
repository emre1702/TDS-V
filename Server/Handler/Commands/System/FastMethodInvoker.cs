using System;
using System.Reflection;
using System.Reflection.Emit;
using TDS_Server.Data.Models;

namespace TDS_Server.Handler.Commands.System
{
    internal class FastMethodInvoker
    {
        internal FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            var dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object),typeof(object[]) },
                             methodInfo.DeclaringType!.Module);
            var ilGenerator = dynamicMethod.GetILGenerator();
            var parameters = methodInfo.GetParameters();
            var paramTypes = new Type[parameters.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                paramTypes[i] = parameters[i].ParameterType;
            }
            var locals = new LocalBuilder[paramTypes.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = ilGenerator.DeclareLocal(paramTypes[i]);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg_1);
                EmitFastInt(ilGenerator, i);
                ilGenerator.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(ilGenerator, paramTypes[i]);
                ilGenerator.Emit(OpCodes.Stloc, locals[i]);
            }
            ilGenerator.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < paramTypes.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldloc, locals[i]);
            }
            ilGenerator.EmitCall(OpCodes.Call, methodInfo, null);
            if (methodInfo.ReturnType == typeof(void))
                ilGenerator.Emit(OpCodes.Ldnull);
            else
                EmitBoxIfNeeded(ilGenerator, methodInfo.ReturnType);
            ilGenerator.Emit(OpCodes.Ret);
            var invoder = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
            return invoder;
        }

        private void EmitCastToReference(ILGenerator il, Type type)
        { 
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

        private void EmitBoxIfNeeded(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }
    }
}
