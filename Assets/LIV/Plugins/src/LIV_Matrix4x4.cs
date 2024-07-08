//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.2
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class LIV_Matrix4x4 : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal LIV_Matrix4x4(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(LIV_Matrix4x4 obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~LIV_Matrix4x4() {
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
          LIV_NativePINVOKE.delete_LIV_Matrix4x4(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public SWIGTYPE_p_float data {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_data_set(swigCPtr, SWIGTYPE_p_float.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = LIV_NativePINVOKE.LIV_Matrix4x4_data_get(swigCPtr);
      SWIGTYPE_p_float ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_float(cPtr, false);
      return ret;
    } 
  }

  public LIV_Vector4 vectors {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_vectors_set(swigCPtr, LIV_Vector4.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = LIV_NativePINVOKE.LIV_Matrix4x4_vectors_get(swigCPtr);
      LIV_Vector4 ret = (cPtr == global::System.IntPtr.Zero) ? null : new LIV_Vector4(cPtr, false);
      return ret;
    } 
  }

  public float m00 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m00_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m00_get(swigCPtr);
      return ret;
    } 
  }

  public float m01 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m01_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m01_get(swigCPtr);
      return ret;
    } 
  }

  public float m02 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m02_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m02_get(swigCPtr);
      return ret;
    } 
  }

  public float m03 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m03_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m03_get(swigCPtr);
      return ret;
    } 
  }

  public float m10 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m10_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m10_get(swigCPtr);
      return ret;
    } 
  }

  public float m11 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m11_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m11_get(swigCPtr);
      return ret;
    } 
  }

  public float m12 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m12_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m12_get(swigCPtr);
      return ret;
    } 
  }

  public float m13 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m13_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m13_get(swigCPtr);
      return ret;
    } 
  }

  public float m20 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m20_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m20_get(swigCPtr);
      return ret;
    } 
  }

  public float m21 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m21_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m21_get(swigCPtr);
      return ret;
    } 
  }

  public float m22 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m22_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m22_get(swigCPtr);
      return ret;
    } 
  }

  public float m23 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m23_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m23_get(swigCPtr);
      return ret;
    } 
  }

  public float m30 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m30_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m30_get(swigCPtr);
      return ret;
    } 
  }

  public float m31 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m31_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m31_get(swigCPtr);
      return ret;
    } 
  }

  public float m32 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m32_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m32_get(swigCPtr);
      return ret;
    } 
  }

  public float m33 {
    set {
      LIV_NativePINVOKE.LIV_Matrix4x4_m33_set(swigCPtr, value);
    } 
    get {
      float ret = LIV_NativePINVOKE.LIV_Matrix4x4_m33_get(swigCPtr);
      return ret;
    } 
  }

  public LIV_Matrix4x4() : this(LIV_NativePINVOKE.new_LIV_Matrix4x4(), true) {
  }

}
