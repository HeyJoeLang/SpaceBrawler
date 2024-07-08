//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.2
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class LIV_RigidTransform : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal LIV_RigidTransform(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(LIV_RigidTransform obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~LIV_RigidTransform() {
    Dispose(false);
  }

  public void Dispose() {
    Dispose(true);
    global::System.GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          LIV_NativePINVOKE.delete_LIV_RigidTransform(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public LIV_Vector3 pos {
    set {
      LIV_NativePINVOKE.LIV_RigidTransform_pos_set(swigCPtr, LIV_Vector3.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = LIV_NativePINVOKE.LIV_RigidTransform_pos_get(swigCPtr);
      LIV_Vector3 ret = (cPtr == global::System.IntPtr.Zero) ? null : new LIV_Vector3(cPtr, false);
      return ret;
    } 
  }

  public LIV_Quaternion rot {
    set {
      LIV_NativePINVOKE.LIV_RigidTransform_rot_set(swigCPtr, LIV_Quaternion.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = LIV_NativePINVOKE.LIV_RigidTransform_rot_get(swigCPtr);
      LIV_Quaternion ret = (cPtr == global::System.IntPtr.Zero) ? null : new LIV_Quaternion(cPtr, false);
      return ret;
    } 
  }

  public LIV_RigidTransform() : this(LIV_NativePINVOKE.new_LIV_RigidTransform(), true) {
  }

}
