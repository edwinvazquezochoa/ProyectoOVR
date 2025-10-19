#define ICALL_TABLE_corlib 1

static int corlib_icall_indexes [] = {
250,
262,
263,
264,
265,
266,
267,
268,
269,
270,
273,
274,
275,
448,
449,
450,
479,
480,
481,
501,
502,
503,
504,
621,
622,
623,
626,
668,
669,
670,
673,
675,
677,
679,
684,
692,
693,
694,
695,
696,
697,
698,
699,
700,
701,
702,
703,
704,
705,
706,
707,
708,
710,
711,
712,
713,
714,
715,
716,
808,
809,
810,
811,
812,
813,
814,
815,
816,
817,
818,
819,
820,
821,
822,
823,
824,
826,
827,
828,
829,
830,
831,
832,
899,
900,
968,
975,
978,
980,
985,
986,
988,
989,
993,
994,
996,
998,
999,
1002,
1003,
1004,
1007,
1009,
1012,
1014,
1016,
1025,
1093,
1095,
1097,
1107,
1108,
1109,
1110,
1112,
1119,
1120,
1121,
1122,
1123,
1131,
1132,
1133,
1137,
1138,
1140,
1144,
1145,
1146,
1430,
1620,
1621,
9850,
9851,
9853,
9854,
9855,
9856,
9857,
9858,
9860,
9862,
9864,
9865,
9866,
9877,
9879,
9887,
9889,
9891,
9893,
9946,
9947,
9949,
9950,
9951,
9952,
9953,
9955,
9957,
11072,
11076,
11078,
11079,
11080,
11081,
11344,
11345,
11346,
11347,
11365,
11366,
11367,
11369,
11412,
11489,
11491,
11493,
11503,
11504,
11505,
11506,
11507,
11952,
11953,
11958,
11959,
11993,
12013,
12020,
12027,
12038,
12042,
12068,
12149,
12151,
12162,
12164,
12165,
12166,
12173,
12188,
12208,
12209,
12217,
12219,
12226,
12227,
12230,
12232,
12237,
12243,
12244,
12251,
12253,
12265,
12268,
12269,
12270,
12281,
12290,
12296,
12297,
12298,
12300,
12301,
12318,
12320,
12334,
12356,
12357,
12382,
12387,
12417,
12418,
13004,
13018,
13113,
13114,
13329,
13330,
13337,
13338,
13339,
13345,
13443,
14006,
14007,
14575,
14576,
14577,
14582,
14592,
15542,
15563,
15565,
15567,
};
void ves_icall_System_Array_InternalCreate (int,int,int,int,int);
int ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal (int);
int ves_icall_System_Array_IsValueOfElementTypeInternal (int,int);
int ves_icall_System_Array_CanChangePrimitive (int,int,int);
int ves_icall_System_Array_FastCopy (int,int,int,int,int);
int ves_icall_System_Array_GetLengthInternal_raw (int,int,int);
int ves_icall_System_Array_GetLowerBoundInternal_raw (int,int,int);
void ves_icall_System_Array_GetGenericValue_icall (int,int,int);
void ves_icall_System_Array_GetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_SetGenericValue_icall (int,int,int);
void ves_icall_System_Array_SetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_InitializeInternal_raw (int,int);
void ves_icall_System_Array_SetValueRelaxedImpl_raw (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_ZeroMemory (int,int);
void ves_icall_System_Runtime_RuntimeImports_Memmove (int,int,int);
void ves_icall_System_Buffer_BulkMoveWithWriteBarrier (int,int,int,int);
int ves_icall_System_Delegate_AllocDelegateLike_internal_raw (int,int);
int ves_icall_System_Delegate_CreateDelegate_internal_raw (int,int,int,int,int);
int ves_icall_System_Delegate_GetVirtualMethod_internal_raw (int,int);
void ves_icall_System_Enum_GetEnumValuesAndNames_raw (int,int,int,int);
void ves_icall_System_Enum_InternalBoxEnum_raw (int,int,int64_t,int);
int ves_icall_System_Enum_InternalGetCorElementType (int);
void ves_icall_System_Enum_InternalGetUnderlyingType_raw (int,int,int);
int ves_icall_System_Environment_get_ProcessorCount ();
int ves_icall_System_Environment_get_TickCount ();
int64_t ves_icall_System_Environment_get_TickCount64 ();
void ves_icall_System_Environment_FailFast_raw (int,int,int,int);
int ves_icall_System_GC_GetCollectionCount (int);
void ves_icall_System_GC_register_ephemeron_array_raw (int,int);
int ves_icall_System_GC_get_ephemeron_tombstone_raw (int);
void ves_icall_System_GC_SuppressFinalize_raw (int,int);
void ves_icall_System_GC_ReRegisterForFinalize_raw (int,int);
void ves_icall_System_GC_GetGCMemoryInfo (int,int,int,int,int,int);
int ves_icall_System_GC_AllocPinnedArray_raw (int,int,int);
int ves_icall_System_Object_MemberwiseClone_raw (int,int);
double ves_icall_System_Math_Acos (double);
double ves_icall_System_Math_Acosh (double);
double ves_icall_System_Math_Asin (double);
double ves_icall_System_Math_Asinh (double);
double ves_icall_System_Math_Atan (double);
double ves_icall_System_Math_Atan2 (double,double);
double ves_icall_System_Math_Atanh (double);
double ves_icall_System_Math_Cbrt (double);
double ves_icall_System_Math_Ceiling (double);
double ves_icall_System_Math_Cos (double);
double ves_icall_System_Math_Cosh (double);
double ves_icall_System_Math_Exp (double);
double ves_icall_System_Math_Floor (double);
double ves_icall_System_Math_Log (double);
double ves_icall_System_Math_Log10 (double);
double ves_icall_System_Math_Pow (double,double);
double ves_icall_System_Math_Sin (double);
double ves_icall_System_Math_Sinh (double);
double ves_icall_System_Math_Sqrt (double);
double ves_icall_System_Math_Tan (double);
double ves_icall_System_Math_Tanh (double);
double ves_icall_System_Math_FusedMultiplyAdd (double,double,double);
double ves_icall_System_Math_Log2 (double);
double ves_icall_System_Math_ModF (double,int);
float ves_icall_System_MathF_Acos (float);
float ves_icall_System_MathF_Acosh (float);
float ves_icall_System_MathF_Asin (float);
float ves_icall_System_MathF_Asinh (float);
float ves_icall_System_MathF_Atan (float);
float ves_icall_System_MathF_Atan2 (float,float);
float ves_icall_System_MathF_Atanh (float);
float ves_icall_System_MathF_Cbrt (float);
float ves_icall_System_MathF_Ceiling (float);
float ves_icall_System_MathF_Cos (float);
float ves_icall_System_MathF_Cosh (float);
float ves_icall_System_MathF_Exp (float);
float ves_icall_System_MathF_Floor (float);
float ves_icall_System_MathF_Log (float);
float ves_icall_System_MathF_Log10 (float);
float ves_icall_System_MathF_Pow (float,float);
float ves_icall_System_MathF_Sin (float);
float ves_icall_System_MathF_Sinh (float);
float ves_icall_System_MathF_Sqrt (float);
float ves_icall_System_MathF_Tan (float);
float ves_icall_System_MathF_Tanh (float);
float ves_icall_System_MathF_FusedMultiplyAdd (float,float,float);
float ves_icall_System_MathF_Log2 (float);
float ves_icall_System_MathF_ModF (float,int);
void ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw (int,int,int);
void ves_icall_RuntimeMethodHandle_ReboxToNullable_raw (int,int,int,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
void ves_icall_RuntimeType_make_array_type_raw (int,int,int,int);
void ves_icall_RuntimeType_make_byref_type_raw (int,int,int);
void ves_icall_RuntimeType_make_pointer_type_raw (int,int,int);
void ves_icall_RuntimeType_MakeGenericType_raw (int,int,int,int);
int ves_icall_RuntimeType_GetMethodsByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetPropertiesByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetConstructors_native_raw (int,int,int);
int ves_icall_System_RuntimeType_CreateInstanceInternal_raw (int,int);
void ves_icall_System_RuntimeType_AllocateValueType_raw (int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringMethod_raw (int,int,int);
void ves_icall_System_RuntimeType_getFullName_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetGenericArgumentsInternal_raw (int,int,int,int);
int ves_icall_RuntimeType_GetGenericParameterPosition (int);
int ves_icall_RuntimeType_GetEvents_native_raw (int,int,int,int);
int ves_icall_RuntimeType_GetFields_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetInterfaces_raw (int,int,int);
int ves_icall_RuntimeType_GetNestedTypes_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringType_raw (int,int,int);
void ves_icall_RuntimeType_GetName_raw (int,int,int);
void ves_icall_RuntimeType_GetNamespace_raw (int,int,int);
int ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetAttributes (int);
int ves_icall_RuntimeTypeHandle_GetMetadataToken_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_GetCorElementType (int);
int ves_icall_RuntimeTypeHandle_HasInstantiation (int);
int ves_icall_RuntimeTypeHandle_IsComObject_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_HasReferences_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetArrayRank_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetAssembly_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetElementType_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetModule_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetBaseType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition (int);
int ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw (int,int);
int ves_icall_RuntimeTypeHandle_is_subclass_of_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsByRefLike_raw (int,int);
void ves_icall_System_RuntimeTypeHandle_internal_from_name_raw (int,int,int,int,int,int);
int ves_icall_System_String_FastAllocateString_raw (int,int);
int ves_icall_System_String_InternalIsInterned_raw (int,int);
int ves_icall_System_String_InternalIntern_raw (int,int);
int ves_icall_System_Type_internal_from_handle_raw (int,int);
int ves_icall_System_ValueType_InternalGetHashCode_raw (int,int,int);
int ves_icall_System_ValueType_Equals_raw (int,int,int,int);
int ves_icall_System_Threading_Interlocked_CompareExchange_Int (int,int,int);
void ves_icall_System_Threading_Interlocked_CompareExchange_Object (int,int,int,int);
int ves_icall_System_Threading_Interlocked_Decrement_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Decrement_Long (int);
int ves_icall_System_Threading_Interlocked_Increment_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Increment_Long (int);
int ves_icall_System_Threading_Interlocked_Exchange_Int (int,int);
void ves_icall_System_Threading_Interlocked_Exchange_Object (int,int,int);
int64_t ves_icall_System_Threading_Interlocked_CompareExchange_Long (int,int64_t,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Exchange_Long (int,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Read_Long (int);
int ves_icall_System_Threading_Interlocked_Add_Int (int,int);
int64_t ves_icall_System_Threading_Interlocked_Add_Long (int,int64_t);
void ves_icall_System_Threading_Monitor_Monitor_Enter_raw (int,int);
void mono_monitor_exit_icall_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_wait_raw (int,int,int,int);
void ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw (int,int,int,int,int);
void ves_icall_System_Threading_Thread_InitInternal_raw (int,int);
int ves_icall_System_Threading_Thread_GetCurrentThread ();
void ves_icall_System_Threading_InternalThread_Thread_free_internal_raw (int,int);
int ves_icall_System_Threading_Thread_GetState_raw (int,int);
void ves_icall_System_Threading_Thread_SetState_raw (int,int,int);
void ves_icall_System_Threading_Thread_ClrState_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetName_icall_raw (int,int,int,int);
int ves_icall_System_Threading_Thread_YieldInternal ();
void ves_icall_System_Threading_Thread_SetPriority_raw (int,int,int);
void ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw (int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw (int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw (int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw (int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw (int);
int ves_icall_System_GCHandle_InternalAlloc_raw (int,int,int);
void ves_icall_System_GCHandle_InternalFree_raw (int,int);
int ves_icall_System_GCHandle_InternalGet_raw (int,int);
void ves_icall_System_GCHandle_InternalSet_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError ();
void ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError (int);
void ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw (int,int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalTryGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw (int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw (int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw (int,int,int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack ();
int ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw (int,int);
int ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_InternalLoad_raw (int,int,int,int);
int ves_icall_System_Reflection_Assembly_InternalGetType_raw (int,int,int,int,int,int);
int ves_icall_System_Reflection_AssemblyName_GetNativeName (int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw (int,int);
int ves_icall_MonoCustomAttrs_IsDefinedInternal_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw (int,int);
int ves_icall_System_Reflection_LoaderAllocatorScout_Destroy (int);
void ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw (int,int,int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw (int,int,int,int,int);
void ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw (int,int,int,int,int,int,int);
void ves_icall_RuntimeEventInfo_get_event_info_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_ResolveType_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetParentType_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_GetFieldOffset_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetValueInternal_raw (int,int,int);
void ves_icall_RuntimeFieldInfo_SetValueInternal_raw (int,int,int,int);
int ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_get_method_info_raw (int,int,int);
int ves_icall_get_method_attributes (int);
int ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw (int,int,int);
int ves_icall_System_MonoMethodInfo_get_retval_marshal_raw (int,int);
int ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw (int,int,int,int);
int ves_icall_RuntimeMethodInfo_get_name_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_base_method_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
void ves_icall_RuntimeMethodInfo_GetPInvoke_raw (int,int,int,int,int);
int ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw (int,int,int);
int ves_icall_RuntimeMethodInfo_GetGenericArguments_raw (int,int);
int ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw (int,int);
void ves_icall_InvokeClassConstructor_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw (int,int,int);
int ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw (int,int,int,int,int,int);
int ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw (int,int,int,int,int,int);
void ves_icall_RuntimePropertyInfo_get_property_info_raw (int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_CustomAttributeBuilder_GetBlob_raw (int,int,int,int,int,int,int,int);
void ves_icall_DynamicMethod_create_dynamic_method_raw (int,int,int,int,int);
void ves_icall_AssemblyBuilder_basic_init_raw (int,int);
void ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw (int,int);
void ves_icall_ModuleBuilder_basic_init_raw (int,int);
void ves_icall_ModuleBuilder_set_wrappers_type_raw (int,int,int);
int ves_icall_ModuleBuilder_getUSIndex_raw (int,int,int);
int ves_icall_ModuleBuilder_getToken_raw (int,int,int,int);
int ves_icall_ModuleBuilder_getMethodToken_raw (int,int,int,int);
void ves_icall_ModuleBuilder_RegisterToken_raw (int,int,int,int);
int ves_icall_TypeBuilder_create_runtime_class_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw (int,int);
int ves_icall_System_Diagnostics_Debugger_IsAttached_internal ();
int ves_icall_System_Diagnostics_Debugger_IsLogging ();
void ves_icall_System_Diagnostics_Debugger_Log (int,int,int);
int ves_icall_System_Diagnostics_StackFrame_GetFrameInfo (int,int,int,int,int,int,int,int);
void ves_icall_System_Diagnostics_StackTrace_GetTrace (int,int,int,int);
int ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass (int);
void ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree (int);
int ves_icall_Mono_SafeStringMarshal_StringToUtf8 (int);
void ves_icall_Mono_SafeStringMarshal_GFree (int);
static void *corlib_icall_funcs [] = {
// token 250,
ves_icall_System_Array_InternalCreate,
// token 262,
ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal,
// token 263,
ves_icall_System_Array_IsValueOfElementTypeInternal,
// token 264,
ves_icall_System_Array_CanChangePrimitive,
// token 265,
ves_icall_System_Array_FastCopy,
// token 266,
ves_icall_System_Array_GetLengthInternal_raw,
// token 267,
ves_icall_System_Array_GetLowerBoundInternal_raw,
// token 268,
ves_icall_System_Array_GetGenericValue_icall,
// token 269,
ves_icall_System_Array_GetValueImpl_raw,
// token 270,
ves_icall_System_Array_SetGenericValue_icall,
// token 273,
ves_icall_System_Array_SetValueImpl_raw,
// token 274,
ves_icall_System_Array_InitializeInternal_raw,
// token 275,
ves_icall_System_Array_SetValueRelaxedImpl_raw,
// token 448,
ves_icall_System_Runtime_RuntimeImports_ZeroMemory,
// token 449,
ves_icall_System_Runtime_RuntimeImports_Memmove,
// token 450,
ves_icall_System_Buffer_BulkMoveWithWriteBarrier,
// token 479,
ves_icall_System_Delegate_AllocDelegateLike_internal_raw,
// token 480,
ves_icall_System_Delegate_CreateDelegate_internal_raw,
// token 481,
ves_icall_System_Delegate_GetVirtualMethod_internal_raw,
// token 501,
ves_icall_System_Enum_GetEnumValuesAndNames_raw,
// token 502,
ves_icall_System_Enum_InternalBoxEnum_raw,
// token 503,
ves_icall_System_Enum_InternalGetCorElementType,
// token 504,
ves_icall_System_Enum_InternalGetUnderlyingType_raw,
// token 621,
ves_icall_System_Environment_get_ProcessorCount,
// token 622,
ves_icall_System_Environment_get_TickCount,
// token 623,
ves_icall_System_Environment_get_TickCount64,
// token 626,
ves_icall_System_Environment_FailFast_raw,
// token 668,
ves_icall_System_GC_GetCollectionCount,
// token 669,
ves_icall_System_GC_register_ephemeron_array_raw,
// token 670,
ves_icall_System_GC_get_ephemeron_tombstone_raw,
// token 673,
ves_icall_System_GC_SuppressFinalize_raw,
// token 675,
ves_icall_System_GC_ReRegisterForFinalize_raw,
// token 677,
ves_icall_System_GC_GetGCMemoryInfo,
// token 679,
ves_icall_System_GC_AllocPinnedArray_raw,
// token 684,
ves_icall_System_Object_MemberwiseClone_raw,
// token 692,
ves_icall_System_Math_Acos,
// token 693,
ves_icall_System_Math_Acosh,
// token 694,
ves_icall_System_Math_Asin,
// token 695,
ves_icall_System_Math_Asinh,
// token 696,
ves_icall_System_Math_Atan,
// token 697,
ves_icall_System_Math_Atan2,
// token 698,
ves_icall_System_Math_Atanh,
// token 699,
ves_icall_System_Math_Cbrt,
// token 700,
ves_icall_System_Math_Ceiling,
// token 701,
ves_icall_System_Math_Cos,
// token 702,
ves_icall_System_Math_Cosh,
// token 703,
ves_icall_System_Math_Exp,
// token 704,
ves_icall_System_Math_Floor,
// token 705,
ves_icall_System_Math_Log,
// token 706,
ves_icall_System_Math_Log10,
// token 707,
ves_icall_System_Math_Pow,
// token 708,
ves_icall_System_Math_Sin,
// token 710,
ves_icall_System_Math_Sinh,
// token 711,
ves_icall_System_Math_Sqrt,
// token 712,
ves_icall_System_Math_Tan,
// token 713,
ves_icall_System_Math_Tanh,
// token 714,
ves_icall_System_Math_FusedMultiplyAdd,
// token 715,
ves_icall_System_Math_Log2,
// token 716,
ves_icall_System_Math_ModF,
// token 808,
ves_icall_System_MathF_Acos,
// token 809,
ves_icall_System_MathF_Acosh,
// token 810,
ves_icall_System_MathF_Asin,
// token 811,
ves_icall_System_MathF_Asinh,
// token 812,
ves_icall_System_MathF_Atan,
// token 813,
ves_icall_System_MathF_Atan2,
// token 814,
ves_icall_System_MathF_Atanh,
// token 815,
ves_icall_System_MathF_Cbrt,
// token 816,
ves_icall_System_MathF_Ceiling,
// token 817,
ves_icall_System_MathF_Cos,
// token 818,
ves_icall_System_MathF_Cosh,
// token 819,
ves_icall_System_MathF_Exp,
// token 820,
ves_icall_System_MathF_Floor,
// token 821,
ves_icall_System_MathF_Log,
// token 822,
ves_icall_System_MathF_Log10,
// token 823,
ves_icall_System_MathF_Pow,
// token 824,
ves_icall_System_MathF_Sin,
// token 826,
ves_icall_System_MathF_Sinh,
// token 827,
ves_icall_System_MathF_Sqrt,
// token 828,
ves_icall_System_MathF_Tan,
// token 829,
ves_icall_System_MathF_Tanh,
// token 830,
ves_icall_System_MathF_FusedMultiplyAdd,
// token 831,
ves_icall_System_MathF_Log2,
// token 832,
ves_icall_System_MathF_ModF,
// token 899,
ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw,
// token 900,
ves_icall_RuntimeMethodHandle_ReboxToNullable_raw,
// token 968,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 975,
ves_icall_RuntimeType_make_array_type_raw,
// token 978,
ves_icall_RuntimeType_make_byref_type_raw,
// token 980,
ves_icall_RuntimeType_make_pointer_type_raw,
// token 985,
ves_icall_RuntimeType_MakeGenericType_raw,
// token 986,
ves_icall_RuntimeType_GetMethodsByName_native_raw,
// token 988,
ves_icall_RuntimeType_GetPropertiesByName_native_raw,
// token 989,
ves_icall_RuntimeType_GetConstructors_native_raw,
// token 993,
ves_icall_System_RuntimeType_CreateInstanceInternal_raw,
// token 994,
ves_icall_System_RuntimeType_AllocateValueType_raw,
// token 996,
ves_icall_RuntimeType_GetDeclaringMethod_raw,
// token 998,
ves_icall_System_RuntimeType_getFullName_raw,
// token 999,
ves_icall_RuntimeType_GetGenericArgumentsInternal_raw,
// token 1002,
ves_icall_RuntimeType_GetGenericParameterPosition,
// token 1003,
ves_icall_RuntimeType_GetEvents_native_raw,
// token 1004,
ves_icall_RuntimeType_GetFields_native_raw,
// token 1007,
ves_icall_RuntimeType_GetInterfaces_raw,
// token 1009,
ves_icall_RuntimeType_GetNestedTypes_native_raw,
// token 1012,
ves_icall_RuntimeType_GetDeclaringType_raw,
// token 1014,
ves_icall_RuntimeType_GetName_raw,
// token 1016,
ves_icall_RuntimeType_GetNamespace_raw,
// token 1025,
ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw,
// token 1093,
ves_icall_RuntimeTypeHandle_GetAttributes,
// token 1095,
ves_icall_RuntimeTypeHandle_GetMetadataToken_raw,
// token 1097,
ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw,
// token 1107,
ves_icall_RuntimeTypeHandle_GetCorElementType,
// token 1108,
ves_icall_RuntimeTypeHandle_HasInstantiation,
// token 1109,
ves_icall_RuntimeTypeHandle_IsComObject_raw,
// token 1110,
ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw,
// token 1112,
ves_icall_RuntimeTypeHandle_HasReferences_raw,
// token 1119,
ves_icall_RuntimeTypeHandle_GetArrayRank_raw,
// token 1120,
ves_icall_RuntimeTypeHandle_GetAssembly_raw,
// token 1121,
ves_icall_RuntimeTypeHandle_GetElementType_raw,
// token 1122,
ves_icall_RuntimeTypeHandle_GetModule_raw,
// token 1123,
ves_icall_RuntimeTypeHandle_GetBaseType_raw,
// token 1131,
ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw,
// token 1132,
ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition,
// token 1133,
ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw,
// token 1137,
ves_icall_RuntimeTypeHandle_is_subclass_of_raw,
// token 1138,
ves_icall_RuntimeTypeHandle_IsByRefLike_raw,
// token 1140,
ves_icall_System_RuntimeTypeHandle_internal_from_name_raw,
// token 1144,
ves_icall_System_String_FastAllocateString_raw,
// token 1145,
ves_icall_System_String_InternalIsInterned_raw,
// token 1146,
ves_icall_System_String_InternalIntern_raw,
// token 1430,
ves_icall_System_Type_internal_from_handle_raw,
// token 1620,
ves_icall_System_ValueType_InternalGetHashCode_raw,
// token 1621,
ves_icall_System_ValueType_Equals_raw,
// token 9850,
ves_icall_System_Threading_Interlocked_CompareExchange_Int,
// token 9851,
ves_icall_System_Threading_Interlocked_CompareExchange_Object,
// token 9853,
ves_icall_System_Threading_Interlocked_Decrement_Int,
// token 9854,
ves_icall_System_Threading_Interlocked_Decrement_Long,
// token 9855,
ves_icall_System_Threading_Interlocked_Increment_Int,
// token 9856,
ves_icall_System_Threading_Interlocked_Increment_Long,
// token 9857,
ves_icall_System_Threading_Interlocked_Exchange_Int,
// token 9858,
ves_icall_System_Threading_Interlocked_Exchange_Object,
// token 9860,
ves_icall_System_Threading_Interlocked_CompareExchange_Long,
// token 9862,
ves_icall_System_Threading_Interlocked_Exchange_Long,
// token 9864,
ves_icall_System_Threading_Interlocked_Read_Long,
// token 9865,
ves_icall_System_Threading_Interlocked_Add_Int,
// token 9866,
ves_icall_System_Threading_Interlocked_Add_Long,
// token 9877,
ves_icall_System_Threading_Monitor_Monitor_Enter_raw,
// token 9879,
mono_monitor_exit_icall_raw,
// token 9887,
ves_icall_System_Threading_Monitor_Monitor_pulse_raw,
// token 9889,
ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw,
// token 9891,
ves_icall_System_Threading_Monitor_Monitor_wait_raw,
// token 9893,
ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw,
// token 9946,
ves_icall_System_Threading_Thread_InitInternal_raw,
// token 9947,
ves_icall_System_Threading_Thread_GetCurrentThread,
// token 9949,
ves_icall_System_Threading_InternalThread_Thread_free_internal_raw,
// token 9950,
ves_icall_System_Threading_Thread_GetState_raw,
// token 9951,
ves_icall_System_Threading_Thread_SetState_raw,
// token 9952,
ves_icall_System_Threading_Thread_ClrState_raw,
// token 9953,
ves_icall_System_Threading_Thread_SetName_icall_raw,
// token 9955,
ves_icall_System_Threading_Thread_YieldInternal,
// token 9957,
ves_icall_System_Threading_Thread_SetPriority_raw,
// token 11072,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw,
// token 11076,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw,
// token 11078,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw,
// token 11079,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw,
// token 11080,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw,
// token 11081,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw,
// token 11344,
ves_icall_System_GCHandle_InternalAlloc_raw,
// token 11345,
ves_icall_System_GCHandle_InternalFree_raw,
// token 11346,
ves_icall_System_GCHandle_InternalGet_raw,
// token 11347,
ves_icall_System_GCHandle_InternalSet_raw,
// token 11365,
ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError,
// token 11366,
ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError,
// token 11367,
ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw,
// token 11369,
ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw,
// token 11412,
ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw,
// token 11489,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw,
// token 11491,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalTryGetHashCode_raw,
// token 11493,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw,
// token 11503,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw,
// token 11504,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw,
// token 11505,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw,
// token 11506,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw,
// token 11507,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack,
// token 11952,
ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw,
// token 11953,
ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw,
// token 11958,
ves_icall_System_Reflection_Assembly_InternalLoad_raw,
// token 11959,
ves_icall_System_Reflection_Assembly_InternalGetType_raw,
// token 11993,
ves_icall_System_Reflection_AssemblyName_GetNativeName,
// token 12013,
ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw,
// token 12020,
ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw,
// token 12027,
ves_icall_MonoCustomAttrs_IsDefinedInternal_raw,
// token 12038,
ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw,
// token 12042,
ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw,
// token 12068,
ves_icall_System_Reflection_LoaderAllocatorScout_Destroy,
// token 12149,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw,
// token 12151,
ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw,
// token 12162,
ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw,
// token 12164,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw,
// token 12165,
ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw,
// token 12166,
ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw,
// token 12173,
ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw,
// token 12188,
ves_icall_RuntimeEventInfo_get_event_info_raw,
// token 12208,
ves_icall_reflection_get_token_raw,
// token 12209,
ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw,
// token 12217,
ves_icall_RuntimeFieldInfo_ResolveType_raw,
// token 12219,
ves_icall_RuntimeFieldInfo_GetParentType_raw,
// token 12226,
ves_icall_RuntimeFieldInfo_GetFieldOffset_raw,
// token 12227,
ves_icall_RuntimeFieldInfo_GetValueInternal_raw,
// token 12230,
ves_icall_RuntimeFieldInfo_SetValueInternal_raw,
// token 12232,
ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw,
// token 12237,
ves_icall_reflection_get_token_raw,
// token 12243,
ves_icall_get_method_info_raw,
// token 12244,
ves_icall_get_method_attributes,
// token 12251,
ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw,
// token 12253,
ves_icall_System_MonoMethodInfo_get_retval_marshal_raw,
// token 12265,
ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw,
// token 12268,
ves_icall_RuntimeMethodInfo_get_name_raw,
// token 12269,
ves_icall_RuntimeMethodInfo_get_base_method_raw,
// token 12270,
ves_icall_reflection_get_token_raw,
// token 12281,
ves_icall_InternalInvoke_raw,
// token 12290,
ves_icall_RuntimeMethodInfo_GetPInvoke_raw,
// token 12296,
ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw,
// token 12297,
ves_icall_RuntimeMethodInfo_GetGenericArguments_raw,
// token 12298,
ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw,
// token 12300,
ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw,
// token 12301,
ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw,
// token 12318,
ves_icall_InvokeClassConstructor_raw,
// token 12320,
ves_icall_InternalInvoke_raw,
// token 12334,
ves_icall_reflection_get_token_raw,
// token 12356,
ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw,
// token 12357,
ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw,
// token 12382,
ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw,
// token 12387,
ves_icall_RuntimePropertyInfo_get_property_info_raw,
// token 12417,
ves_icall_reflection_get_token_raw,
// token 12418,
ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw,
// token 13004,
ves_icall_CustomAttributeBuilder_GetBlob_raw,
// token 13018,
ves_icall_DynamicMethod_create_dynamic_method_raw,
// token 13113,
ves_icall_AssemblyBuilder_basic_init_raw,
// token 13114,
ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw,
// token 13329,
ves_icall_ModuleBuilder_basic_init_raw,
// token 13330,
ves_icall_ModuleBuilder_set_wrappers_type_raw,
// token 13337,
ves_icall_ModuleBuilder_getUSIndex_raw,
// token 13338,
ves_icall_ModuleBuilder_getToken_raw,
// token 13339,
ves_icall_ModuleBuilder_getMethodToken_raw,
// token 13345,
ves_icall_ModuleBuilder_RegisterToken_raw,
// token 13443,
ves_icall_TypeBuilder_create_runtime_class_raw,
// token 14006,
ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw,
// token 14007,
ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw,
// token 14575,
ves_icall_System_Diagnostics_Debugger_IsAttached_internal,
// token 14576,
ves_icall_System_Diagnostics_Debugger_IsLogging,
// token 14577,
ves_icall_System_Diagnostics_Debugger_Log,
// token 14582,
ves_icall_System_Diagnostics_StackFrame_GetFrameInfo,
// token 14592,
ves_icall_System_Diagnostics_StackTrace_GetTrace,
// token 15542,
ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass,
// token 15563,
ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree,
// token 15565,
ves_icall_Mono_SafeStringMarshal_StringToUtf8,
// token 15567,
ves_icall_Mono_SafeStringMarshal_GFree,
};
static uint8_t corlib_icall_flags [] = {
0,
0,
0,
0,
0,
4,
4,
0,
4,
0,
4,
4,
4,
0,
0,
0,
4,
4,
4,
4,
4,
0,
4,
0,
0,
0,
4,
0,
4,
4,
4,
4,
0,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
};
