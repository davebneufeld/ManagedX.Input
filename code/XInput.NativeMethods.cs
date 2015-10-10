﻿using System;
using System.Runtime.InteropServices;
using System.Security;


namespace ManagedX.Input.XInput
{

	/// <summary>Provides access to the XInput API functions.</summary>
	[SuppressUnmanagedCodeSecurity]
	internal static class NativeMethods
	{

		// https://msdn.microsoft.com/en-us/library/windows/desktop/ee417005%28v=vs.85%29.aspx


		internal const string LibraryName15 = "XInput1_5.dll";	// Windows 10
		internal const string LibraryName14 = "XInput1_4.dll";	// Windows 8, 8.1
		internal const string LibraryName13 = "XInput1_3.dll";	// Windows Vista, 7


		#region XInput 1.5

		/// <summary>Sets the reporting state of XInput.</summary>
		/// <param name="enable">If set to false, XInput will only send neutral data in response to <see cref="XInput15GetState"/> (all buttons up, axes centered, and triggers at 0).
		/// <see cref="XInput15SetState"/> calls will be registered but not sent to the device.
		/// Sending any value other than false will restore reading and writing functionality to normal.
		/// </param>
		[DllImport( LibraryName15, EntryPoint = "XInputEnable", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern void XInput15Enable(
			[In, MarshalAs( UnmanagedType.Bool )] bool enable
		);


		/// <summary>Retrieves the capabilities and features of a connected controller.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="flags">Input flags that identify the controller type. If this value is 0, then the capabilities of all controllers connected to the system are returned. Currently, only one value is supported: 1.</param>
		/// <param name="capabilities">A <see cref="Capabilities"/> structure that receives the controller capabilities.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</para>
		/// <para>If the controller is not connected, the return value is <see cref="ErrorCode.NotConnected"/>.</para>
		/// <para>If the function fails, the return value is an <seealso cref="ErrorCode">error code</seealso> (defined in WinError.h).</para>
		/// </returns>
		/// <remarks>The legacy XINPUT 9.1.0 version (included in Windows Vista and later) always returned a fixed set of capabilities regardless of attached device.</remarks>
		[DllImport( LibraryName15, EntryPoint = "XInputGetCapabilities", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput15GetCapabilities(
			[In] GameControllerIndex userIndex,
			[In] int flags,
			[Out] out Capabilities capabilities
		);


		/// <summary>Retrieves the battery type and charge status of a wireless controller.
		/// Beware that this function might not be available on Windows Vista.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="deviceType">Specifies which device associated with this user index should be queried.</param>
		/// <param name="batteryInformation">A <see cref="BatteryInformation"/> structure that receives the battery information.</param>
		/// <returns>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</returns>
		[DllImport( LibraryName15, EntryPoint = "XInputGetBatteryInformation", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput15GetBatteryInformation(
			[In] GameControllerIndex userIndex,
			[In] BatteryDeviceType deviceType,
			[Out] out BatteryInformation batteryInformation
		);


		/// <summary>Retrieves the current state of the specified controller.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="state">A <see cref="State"/> structure that receives the current state of the controller.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</para>
		/// <para>If the controller is not connected, the return value is <see cref="ErrorCode.NotConnected"/>.</para>
		/// <para>If the function fails, the return value is an <see cref="ErrorCode">error code</see> (defined in WinError.h).</para>
		/// </returns>
		/// <remarks>
		/// When XInputGetState is used to retrieve controller data, the left and right triggers are each reported separately.
		/// For legacy reasons, when DirectInput retrieves controller data, the two triggers share the same axis.
		/// The legacy behavior is noticeable in the current Game Device Control Panel, which uses DirectInput for controller state.
		/// </remarks>
		[DllImport( LibraryName15, EntryPoint = "XInputGetState", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput15GetState(
			[In] GameControllerIndex userIndex,
			[Out] out State state
		);


		/// <summary>Sends data to a connected controller.
		/// This function is used to activate the vibration function of a controller.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="vibration">A <see cref="Vibration"/> structure containing the vibration information to send to the controller.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</para>
		/// <para>If the controller is not connected, the return value is <see cref="ErrorCode.NotConnected"/>.</para>
		/// <para>If the function fails, the return value is an <see cref="ErrorCode">error code defined in Winerror.h</see>.</para>
		/// </returns>
		[DllImport( LibraryName15, EntryPoint = "XInputSetState", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput15SetState(
			[In] GameControllerIndex userIndex,
			[In] ref Vibration vibration
		);


		/// <summary>Retrieves a gamepad input event.</summary>
		/// <param name="userIndex">Index of the user's controller. Can be a <see cref="GameControllerIndex"/> value, or XUSER_INDEX_ANY(255) to fetch the next available input event from any user.</param>
		/// <param name="reserved">Must be set to 0.</param>
		/// <param name="keystroke">Receives a <see cref="Keystroke"/> structure for an input event.</param>
		/// <returns>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.
		/// If no new keys have been pressed, the return value is <see cref="ErrorCode.Empty"/>.
		/// If the controller is not connected or the user has not activated it, the return value is <see cref="ErrorCode.NotConnected"/>.
		/// If the function fails, the return value is an error code defined in Winerror.h.
		/// </returns>
		[DllImport( LibraryName15, EntryPoint = "XInputGetKeystroke", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput15GetKeystroke(
			[In] GameControllerIndex userIndex,
			[In] int reserved,
			[Out] out Keystroke keystroke
		);


		/// <summary>Retrieves the sound rendering and sound capture audio device IDs that are associated with the headset connected to the specified controller.</summary>
		/// <param name="userIndex">Index of the gamer associated with the device.</param>
		/// <param name="renderDeviceId">Windows Core Audio device ID string for render (speakers).</param>
		/// <param name="renderDeviceIdLength">Size, in wide-chars, of the render device ID string buffer.</param>
		/// <param name="captureDeviceId">Windows Core Audio device ID string for capture (microphone).</param>
		/// <param name="captureDeviceIdLength">Size, in wide-chars, of capture device ID string buffer.</param>
		/// <returns>If the function successfully retrieves the device IDs for render and capture, the return code is <see cref="ErrorCode.None"/>.
		/// If there is no headset connected to the controller, the function will also return <see cref="ErrorCode.None"/> with null as the values for <paramref name="renderDeviceId"/> and <paramref name="captureDeviceId"/>.
		/// If the controller port device is not physically connected, the function will return <see cref="ErrorCode.NotConnected"/>.
		/// If the function fails, it will return a valid Win32 error code.
		/// </returns>
		[DllImport( LibraryName15, EntryPoint = "XInputGetAudioDeviceIds", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput15GetAudioDeviceIds(
			[In] GameControllerIndex userIndex,
			[Out, MarshalAs( UnmanagedType.LPWStr, SizeParamIndex = 2 ), Optional] out string renderDeviceId,
			[In, Out, Optional] ref int renderDeviceIdLength,
			[Out, MarshalAs( UnmanagedType.LPWStr, SizeParamIndex = 4 ), Optional] out string captureDeviceId,
			[In, Out, Optional] ref int captureDeviceIdLength
		);
		
		#endregion


		#region XInput 1.4

		/// <summary>Sets the reporting state of XInput.</summary>
		/// <param name="enable">If set to false, XInput will only send neutral data in response to <see cref="XInput14GetState"/> (all buttons up, axes centered, and triggers at 0).
		/// <see cref="XInput14SetState"/> calls will be registered but not sent to the device.
		/// Sending any value other than false will restore reading and writing functionality to normal.
		/// </param>
		[DllImport( LibraryName14, EntryPoint = "XInputEnable", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern void XInput14Enable(
			[In, MarshalAs( UnmanagedType.Bool )] bool enable
		);

	
		/// <summary>Retrieves the capabilities and features of a connected controller.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="flags">Input flags that identify the controller type. If this value is 0, then the capabilities of all controllers connected to the system are returned. Currently, only one value is supported: 1.</param>
		/// <param name="capabilities">A <see cref="Capabilities"/> structure that receives the controller capabilities.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</para>
		/// <para>If the controller is not connected, the return value is <see cref="ErrorCode.NotConnected"/>.</para>
		/// <para>If the function fails, the return value is an <seealso cref="ErrorCode">error code</seealso> (defined in WinError.h).</para>
		/// </returns>
		/// <remarks>The legacy XINPUT 9.1.0 version (included in Windows Vista and later) always returned a fixed set of capabilities regardless of attached device.</remarks>
		[DllImport( LibraryName14, EntryPoint = "XInputGetCapabilities", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput14GetCapabilities(
			[In] GameControllerIndex userIndex,
			[In] int flags,
			[Out] out Capabilities capabilities
		);


		/// <summary>Retrieves the battery type and charge status of a wireless controller.
		/// Beware that this function might not be available on Windows Vista.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="deviceType">Specifies which device associated with this user index should be queried.</param>
		/// <param name="batteryInformation">A <see cref="BatteryInformation"/> structure that receives the battery information.</param>
		/// <returns>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</returns>
		[DllImport( LibraryName14, EntryPoint = "XInputGetBatteryInformation", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput14GetBatteryInformation(
			[In] GameControllerIndex userIndex,
			[In] BatteryDeviceType deviceType,
			[Out] out BatteryInformation batteryInformation
		);


		/// <summary>Retrieves the current state of the specified controller.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="state">A <see cref="State"/> structure that receives the current state of the controller.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</para>
		/// <para>If the controller is not connected, the return value is <see cref="ErrorCode.NotConnected"/>.</para>
		/// <para>If the function fails, the return value is an <see cref="ErrorCode">error code</see> (defined in WinError.h).</para>
		/// </returns>
		/// <remarks>
		/// When XInputGetState is used to retrieve controller data, the left and right triggers are each reported separately.
		/// For legacy reasons, when DirectInput retrieves controller data, the two triggers share the same axis.
		/// The legacy behavior is noticeable in the current Game Device Control Panel, which uses DirectInput for controller state.
		/// </remarks>
		[DllImport( LibraryName14, EntryPoint = "XInputGetState", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput14GetState(
			[In] GameControllerIndex userIndex,
			[Out] out State state
		);


		/// <summary>Sends data to a connected controller.
		/// This function is used to activate the vibration function of a controller.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="vibration">A <see cref="Vibration"/> structure containing the vibration information to send to the controller.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</para>
		/// <para>If the controller is not connected, the return value is <see cref="ErrorCode.NotConnected"/>.</para>
		/// <para>If the function fails, the return value is an <see cref="ErrorCode">error code defined in Winerror.h</see>.</para>
		/// </returns>
		[DllImport( LibraryName14, EntryPoint = "XInputSetState", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput14SetState(
			[In] GameControllerIndex userIndex,
			[In] ref Vibration vibration
		);

		/// <summary>Retrieves a gamepad input event.</summary>
		/// <param name="userIndex">Index of the user's controller. Can be a <see cref="GameControllerIndex"/> value, or XUSER_INDEX_ANY(255) to fetch the next available input event from any user.</param>
		/// <param name="reserved">Must be set to 0.</param>
		/// <param name="keystroke">Receives a <see cref="Keystroke"/> structure for an input event.</param>
		/// <returns>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.
		/// If no new keys have been pressed, the return value is <see cref="ErrorCode.Empty"/>.
		/// If the controller is not connected or the user has not activated it, the return value is <see cref="ErrorCode.NotConnected"/>.
		/// If the function fails, the return value is an error code defined in Winerror.h.
		/// </returns>
		[DllImport( LibraryName14, EntryPoint = "XInputGetKeystroke", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput14GetKeystroke(
			[In] GameControllerIndex userIndex,
			[In] int reserved,
			[Out] out Keystroke keystroke
		);


		/// <summary>Retrieves the sound rendering and sound capture audio device IDs that are associated with the headset connected to the specified controller.</summary>
		/// <param name="userIndex">Index of the gamer associated with the device.</param>
		/// <param name="renderDeviceId">Windows Core Audio device ID string for render (speakers).</param>
		/// <param name="renderDeviceIdLength">Size, in wide-chars, of the render device ID string buffer.</param>
		/// <param name="captureDeviceId">Windows Core Audio device ID string for capture (microphone).</param>
		/// <param name="captureDeviceIdLength">Size, in wide-chars, of capture device ID string buffer.</param>
		/// <returns>If the function successfully retrieves the device IDs for render and capture, the return code is <see cref="ErrorCode.None"/>.
		/// If there is no headset connected to the controller, the function will also return <see cref="ErrorCode.None"/> with null as the values for <paramref name="renderDeviceId"/> and <paramref name="captureDeviceId"/>.
		/// If the controller port device is not physically connected, the function will return <see cref="ErrorCode.NotConnected"/>.
		/// If the function fails, it will return a valid Win32 error code.
		/// </returns>
		[DllImport( LibraryName14, EntryPoint = "XInputGetAudioDeviceIds", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput14GetAudioDeviceIds(
			[In] GameControllerIndex userIndex,
			[Out, MarshalAs( UnmanagedType.LPWStr, SizeParamIndex = 2 ), Optional] out string renderDeviceId,
			[In, Out, Optional] ref int renderDeviceIdLength,
			[Out, MarshalAs( UnmanagedType.LPWStr, SizeParamIndex = 4 ), Optional] out string captureDeviceId,
			[In, Out, Optional] ref int captureDeviceIdLength
		);
		
		#endregion


		#region XInput 1.3

		/// <summary>Sets the reporting state of XInput.</summary>
		/// <param name="enable">If set to false, XInput will only send neutral data in response to <see cref="XInput14GetState"/> (all buttons up, axes centered, and triggers at 0).
		/// <see cref="XInput14SetState"/> calls will be registered but not sent to the device.
		/// Sending any value other than false will restore reading and writing functionality to normal.
		/// </param>
		[DllImport( LibraryName13, EntryPoint = "XInputEnable", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern void XInput13Enable(
			[In, MarshalAs( UnmanagedType.Bool )] bool enable
		);


		/// <summary>Retrieves the capabilities and features of a connected controller.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="flags">Input flags that identify the controller type. If this value is 0, then the capabilities of all controllers connected to the system are returned. Currently, only one value is supported: 1.</param>
		/// <param name="capabilities">A <see cref="Capabilities"/> structure that receives the controller capabilities.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</para>
		/// <para>If the controller is not connected, the return value is <see cref="ErrorCode.NotConnected"/>.</para>
		/// <para>If the function fails, the return value is an <seealso cref="ErrorCode">error code</seealso> (defined in WinError.h).</para>
		/// </returns>
		/// <remarks>The legacy XINPUT 9.1.0 version (included in Windows Vista and later) always returned a fixed set of capabilities regardless of attached device.</remarks>
		[DllImport( LibraryName13, EntryPoint = "XInputGetCapabilities", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput13GetCapabilities(
			[In] GameControllerIndex userIndex,
			[In] int flags,
			[Out] out Capabilities capabilities
		);


		/// <summary>Retrieves the battery type and charge status of a wireless controller.
		/// Beware that this function might not be available on Windows Vista.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="deviceType">Specifies which device associated with this user index should be queried.</param>
		/// <param name="batteryInformation">A <see cref="BatteryInformation"/> structure that receives the battery information.</param>
		/// <returns>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</returns>
		[DllImport( LibraryName13, EntryPoint = "XInputGetBatteryInformation", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput13GetBatteryInformation(
			[In] GameControllerIndex userIndex,
			[In] BatteryDeviceType deviceType,
			[Out] out BatteryInformation batteryInformation
		);


		/// <summary>Retrieves the current state of the specified controller.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="state">A <see cref="State"/> structure that receives the current state of the controller.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</para>
		/// <para>If the controller is not connected, the return value is <see cref="ErrorCode.NotConnected"/>.</para>
		/// <para>If the function fails, the return value is an <see cref="ErrorCode">error code</see> (defined in WinError.h).</para>
		/// </returns>
		/// <remarks>
		/// When XInputGetState is used to retrieve controller data, the left and right triggers are each reported separately.
		/// For legacy reasons, when DirectInput retrieves controller data, the two triggers share the same axis.
		/// The legacy behavior is noticeable in the current Game Device Control Panel, which uses DirectInput for controller state.
		/// </remarks>
		[DllImport( LibraryName13, EntryPoint = "XInputGetState", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput13GetState(
			[In] GameControllerIndex userIndex,
			[Out] out State state
		);


		/// <summary>Sends data to a connected controller.
		/// This function is used to activate the vibration function of a controller.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="vibration">A <see cref="Vibration"/> structure containing the vibration information to send to the controller.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.</para>
		/// <para>If the controller is not connected, the return value is <see cref="ErrorCode.NotConnected"/>.</para>
		/// <para>If the function fails, the return value is an <see cref="ErrorCode">error code defined in Winerror.h</see>.</para>
		/// </returns>
		[DllImport( LibraryName13, EntryPoint = "XInputSetState", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput13SetState(
			[In] GameControllerIndex userIndex,
			[In] ref Vibration vibration
		);


		/// <summary>Retrieves a gamepad input event.</summary>
		/// <param name="userIndex">Index of the user's controller. Can be a <see cref="GameControllerIndex"/> value, or XUSER_INDEX_ANY(255) to fetch the next available input event from any user.</param>
		/// <param name="reserved">Must be set to 0.</param>
		/// <param name="keystroke">Receives a <see cref="Keystroke"/> structure for an input event.</param>
		/// <returns>If the function succeeds, the return value is <see cref="ErrorCode.None"/>.
		/// If no new keys have been pressed, the return value is <see cref="ErrorCode.Empty"/>.
		/// If the controller is not connected or the user has not activated it, the return value is <see cref="ErrorCode.NotConnected"/>.
		/// If the function fails, the return value is an error code defined in Winerror.h.
		/// </returns>
		[DllImport( LibraryName13, EntryPoint = "XInputGetKeystroke", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput13GetKeystroke(
			[In] GameControllerIndex userIndex,
			[In] int reserved,
			[Out] out Keystroke keystroke
		);


		/// <summary>Gets the sound rendering and sound capture device GUIDs that are associated with the headset connected to the specified controller.</summary>
		/// <param name="userIndex">Index of the user's controller.</param>
		/// <param name="dSoundRenderGuid">Receives the <see cref="Guid"/> of the headset sound rendering device.</param>
		/// <param name="dSoundCaptureGuid">Receives the <see cref="Guid"/> of the headset sound capture device.</param>
		/// <returns>If the function successfully retrieves the device IDs for render and capture, the return code is <see cref="ErrorCode.None"/>.
		/// If there is no headset connected to the controller, the function also retrieves <see cref="ErrorCode.None"/> with <see cref="Guid.Empty"/> as the values for <paramref name="dSoundRenderGuid"/> and <paramref name="dSoundCaptureGuid"/>.
		/// If the controller port device is not physically connected, the function returns <see cref="ErrorCode.NotConnected"/>.
		/// If the function fails, it returns a valid Win32 error code.</returns>
		[DllImport( LibraryName13, EntryPoint = "XInputGetDSoundAudioDeviceGuids", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = true, SetLastError = false )]
		internal static extern int XInput13GetDSoundAudioDeviceGuids(
			[In] GameControllerIndex userIndex,
			[Out] out Guid dSoundRenderGuid,
			[Out] out Guid dSoundCaptureGuid
		);

		#endregion


		/// <summary></summary>
		/// <param name="userIndex"></param>
		/// <param name="dSoundRenderGuid"></param>
		/// <param name="dSoundCaptureGuid"></param>
		/// <returns></returns>
		internal static int XInput15GetDSoundAudioDeviceGuids(
			GameControllerIndex userIndex,
			out Guid dSoundRenderGuid,
			out Guid dSoundCaptureGuid
		)
		{
			// THINKABOUTME - can we use the 1.3 function ?
			dSoundCaptureGuid = dSoundRenderGuid = Guid.Empty;
			Capabilities caps;
			return XInput15GetCapabilities( userIndex, 1, out caps );
		}

		/// <summary></summary>
		/// <param name="userIndex"></param>
		/// <param name="dSoundRenderGuid"></param>
		/// <param name="dSoundCaptureGuid"></param>
		/// <returns></returns>
		internal static int XInput14GetDSoundAudioDeviceGuids(
			GameControllerIndex userIndex,
			out Guid dSoundRenderGuid,
			out Guid dSoundCaptureGuid
		)
		{
			// THINKABOUTME - can we use the 1.3 function ?
			dSoundCaptureGuid = dSoundRenderGuid = Guid.Empty;
			Capabilities caps;
			return XInput14GetCapabilities( userIndex, 1, out caps );
		}

		/// <summary></summary>
		/// <param name="userIndex"></param>
		/// <param name="renderDeviceId"></param>
		/// <param name="renderDeviceIdLength"></param>
		/// <param name="captureDeviceId"></param>
		/// <param name="captureDeviceIdLength"></param>
		/// <returns></returns>
		internal static int XInput13GetAudioDeviceIds(
			GameControllerIndex userIndex,
			out string renderDeviceId,
			ref int renderDeviceIdLength,
			out string captureDeviceId,
			ref int captureDeviceIdLength
		)
		{
			renderDeviceId = captureDeviceId = null;
			renderDeviceIdLength = captureDeviceIdLength = 0;
			Capabilities caps;
			return XInput13GetCapabilities( userIndex, 1, out caps );
		}

	}

}