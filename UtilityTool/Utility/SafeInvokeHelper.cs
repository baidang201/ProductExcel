using System;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace UtilityTool
{

    public  static class SafeInvokeHelper
    {
        static readonly ModuleBuilder builder;
        static readonly AssemblyBuilder myAsmBuilder;
        static readonly Hashtable methodLookup;
		static readonly Hashtable delLookup;
        static readonly MemberFilter methodFilter;
		private static DateTime lastCleanTime ;

        static SafeInvokeHelper()
        {
            AssemblyName name = new AssemblyName();
            name.Name = "temp";
            myAsmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            builder = myAsmBuilder.DefineDynamicModule("TempModule");
            methodLookup = new Hashtable();
			delLookup=new Hashtable();
            methodFilter = new MemberFilter(FiltByMethodName);
			lastCleanTime = DateTime.Now;
        }

        private static bool FiltByMethodName(MemberInfo m,Object filterCriteria)
        {
            if (m.MemberType != MemberTypes.Method) return false;
            object[] signatures = (object[])filterCriteria;
            string methodName = (string)signatures[0];
            MethodInfo methInfo = (MethodInfo)m;
            if (methInfo.Name.Equals(methodName))
            {
                ParameterInfo[] paramInfos = methInfo.GetParameters();
                if (paramInfos.Length != signatures.Length - 1) return false;
                int i = 0;
                for (i = 0; i < paramInfos.Length; ++i)
                {
                    if (signatures[i + 1] != null)
                        if (paramInfos[i].ParameterType != signatures[i + 1].GetType() &&
                                !signatures[i + 1].GetType().IsSubclassOf(paramInfos[i].ParameterType))
                        {
                            #region 如果调用方法的参数是一个接口，并且传入参数实现了这个接口，应该也能传进去才对
                            Type[] types = signatures[i + 1].GetType().GetInterfaces();
                            bool haveInterFace = false;
                            foreach (Type type in types)
                            {
                                if (type.Equals(paramInfos[i].ParameterType))
                                {
                                    haveInterFace = true;
                                }
                            }
                            if (haveInterFace == false)
                            #endregion
                                return false;
                        }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public static object Invoke(System.Windows.Forms.Control obj, string methodName, params object[] paramValues)
        {
            Delegate del = null;
			DateTime now  = DateTime.Now;
			TimeSpan ts = now - lastCleanTime;
			if(ts.TotalHours>5)
			{
				delLookup.Clear();
				lastCleanTime = now;
			}
			string delKey = obj.GetType().Name+"."+methodName+obj.GetHashCode().ToString() + obj.ToString();
			if(delLookup.ContainsKey(delKey))
			{
				del =(Delegate) delLookup[delKey];
			}
			else
			{
				string key = obj.GetType().Name + "." + methodName;
				Type tp;
				if (methodLookup.Contains(key))
				{
					tp = (Type)methodLookup[key];
				}
				else
				{
					Type objType = obj.GetType();
					object[] signatures = new object[paramValues.Length + 1];
					signatures[0] = methodName;
					Array.Copy(paramValues, 0, signatures, 1, paramValues.Length);
					MemberInfo[] mInfo = objType.FindMembers(MemberTypes.Method, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, methodFilter, signatures);
					if (mInfo.Length != 1) return null;
					MethodInfo methInfo = (MethodInfo)mInfo[0];
					Type[] paramList = new Type[methInfo.GetParameters().Length];
					ParameterInfo[] paramInfos = methInfo.GetParameters();
					int n = 0;
					foreach (ParameterInfo pi in paramInfos)
					{
						paramList[n++] = pi.ParameterType;
					}
					TypeBuilder typeB = builder.DefineType("Del_" +
						obj.GetType().Name + "_" + methodName,
						TypeAttributes.Class | TypeAttributes.AutoLayout | TypeAttributes.Public | TypeAttributes.Sealed,
						typeof(MulticastDelegate), PackingSize.Unspecified);
					ConstructorBuilder conB = typeB.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.SpecialName
						| MethodAttributes.RTSpecialName, CallingConventions.Standard,
						new Type[] { typeof(object), typeof(IntPtr) });
					conB.SetImplementationFlags(MethodImplAttributes.Runtime);
					MethodBuilder mb = typeB.DefineMethod("Invoke",
						MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
						methInfo.ReturnType, paramList);
					mb.SetImplementationFlags(MethodImplAttributes.Runtime);
					tp = typeB.CreateType();
					methodLookup.Add(key, tp);
				}
				del = MulticastDelegate.CreateDelegate(tp, obj, methodName);
				delLookup.Add(delKey, del);
			}

            return obj.Invoke(del, paramValues);
        }
        private static object FireSingleEvent(Delegate eventDelegate, params object[] paramValues)
        {
            MethodInfo methInfo = eventDelegate.Method;
            object target = eventDelegate.Target;
            if (target is System.Windows.Forms.Control)
            {
				System.Windows.Forms.Control ctrl=(System.Windows.Forms.Control)target;
				if(ctrl.InvokeRequired)
				{
					return SafeInvokeHelper.Invoke(ctrl , methInfo.Name, paramValues);
				}
				else
				{
					return eventDelegate.DynamicInvoke(paramValues);
				}
            }
            else
            {
                return eventDelegate.DynamicInvoke(paramValues);
            }
        }
        public static object FireEvent(Delegate eventDelegate, params object[] paramValues)
        {
            if (eventDelegate == null) return null;
            if (eventDelegate is MulticastDelegate)
            {
                Delegate[] delegates = eventDelegate.GetInvocationList();
                if (delegates.Length > 1)
                {
                    foreach (Delegate eventSink in delegates)
                    {
                        FireEvent(eventSink, paramValues);
                    }
                    return null;
                }
                else
                {
                    return FireSingleEvent(eventDelegate, paramValues);
                }
            }else
            {
                return FireSingleEvent(eventDelegate, paramValues);
            }
        }
    }
}