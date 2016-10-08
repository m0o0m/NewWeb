// Generated by ProtoGen, Version=2.4.1.555, Culture=neutral, PublicKeyToken=55f7125234beb589.  DO NOT EDIT!
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.ProtocolBuffers;
using pbc = global::Google.ProtocolBuffers.Collections;
using pbd = global::Google.ProtocolBuffers.Descriptors;
using scg = global::System.Collections.Generic;
namespace ProtoCmd.BackRecharge {
  
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public static partial class BackRecharge {
  
    #region Extension registration
    public static void RegisterAllExtensions(pb::ExtensionRegistry registry) {
    }
    #endregion
    #region Static variables
    internal static pbd::MessageDescriptor internal__static_ProtoCmd_BackRecharge_normal__Descriptor;
    internal static pb::FieldAccess.FieldAccessorTable<global::ProtoCmd.BackRecharge.normal, global::ProtoCmd.BackRecharge.normal.Builder> internal__static_ProtoCmd_BackRecharge_normal__FieldAccessorTable;
    internal static pbd::MessageDescriptor internal__static_ProtoCmd_BackRecharge_normalRep__Descriptor;
    internal static pb::FieldAccess.FieldAccessorTable<global::ProtoCmd.BackRecharge.normalRep, global::ProtoCmd.BackRecharge.normalRep.Builder> internal__static_ProtoCmd_BackRecharge_normalRep__FieldAccessorTable;
    #endregion
    #region Descriptor
    public static pbd::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbd::FileDescriptor descriptor;
    
    static BackRecharge() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChJCYWNrUmVjaGFyZ2UucHJvdG8SFVByb3RvQ21kLkJhY2tSZWNoYXJnZSJq", 
            "CgZub3JtYWwSDgoGdXNlcklEGAEgAigNEgwKBGxpc3QYAiACKAkSDAoEcm1i", 
            "XxgDIAIoDRIQCghmaXJzdEdpZhgEIAIoCBIOCgZiaWxsTm8YBSABKAkSEgoK", 
            "cm1iX2FjdHVhbBgGIAEoDSIoCglub3JtYWxSZXASDgoGdXNlcklEGAEgAigN", 
            "EgsKA3N1YxgCIAIoCCozCgZCUl9DbWQSDAoIQlJfQkVHSU4QCxINCglCUl9O", 
            "T1JNQUwQDRIMCghCUl9UT1RBTBAUKkwKCUNlbnRlckNtZBIMCghDU19CRUdJ", 
            "ThALEhQKEENTX0NPTk5FQ1RfRVJST1IQDBINCglDU19OT1JNQUwQDRIMCghD", 
          "U19UT1RBTBBk"));
      pbd::FileDescriptor.InternalDescriptorAssigner assigner = delegate(pbd::FileDescriptor root) {
        descriptor = root;
        internal__static_ProtoCmd_BackRecharge_normal__Descriptor = Descriptor.MessageTypes[0];
        internal__static_ProtoCmd_BackRecharge_normal__FieldAccessorTable = 
            new pb::FieldAccess.FieldAccessorTable<global::ProtoCmd.BackRecharge.normal, global::ProtoCmd.BackRecharge.normal.Builder>(internal__static_ProtoCmd_BackRecharge_normal__Descriptor,
                new string[] { "UserID", "List", "Rmb", "FirstGif", "BillNo", "RmbActual", });
        internal__static_ProtoCmd_BackRecharge_normalRep__Descriptor = Descriptor.MessageTypes[1];
        internal__static_ProtoCmd_BackRecharge_normalRep__FieldAccessorTable = 
            new pb::FieldAccess.FieldAccessorTable<global::ProtoCmd.BackRecharge.normalRep, global::ProtoCmd.BackRecharge.normalRep.Builder>(internal__static_ProtoCmd_BackRecharge_normalRep__Descriptor,
                new string[] { "UserID", "Suc", });
        return null;
      };
      pbd::FileDescriptor.InternalBuildGeneratedFileFrom(descriptorData,
          new pbd::FileDescriptor[] {
          }, assigner);
    }
    #endregion
    
  }
  #region Enums
  public enum BR_Cmd {
    BR_BEGIN = 11,
    BR_NORMAL = 13,
    BR_TOTAL = 20,
  }
  
  public enum CenterCmd {
    CS_BEGIN = 11,
    CS_CONNECT_ERROR = 12,
    CS_NORMAL = 13,
    CS_TOTAL = 100,
  }
  
  #endregion
  
  #region Messages
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public sealed partial class normal : pb::GeneratedMessage<normal, normal.Builder> {
    private normal() { }
    private static readonly normal defaultInstance = new normal().MakeReadOnly();
    private static readonly string[] _normalFieldNames = new string[] { "billNo", "firstGif", "list", "rmb_", "rmb_actual", "userID" };
    private static readonly uint[] _normalFieldTags = new uint[] { 42, 32, 18, 24, 48, 8 };
    public static normal DefaultInstance {
      get { return defaultInstance; }
    }
    
    public override normal DefaultInstanceForType {
      get { return DefaultInstance; }
    }
    
    protected override normal ThisMessage {
      get { return this; }
    }
    
    public static pbd::MessageDescriptor Descriptor {
      get { return global::ProtoCmd.BackRecharge.BackRecharge.internal__static_ProtoCmd_BackRecharge_normal__Descriptor; }
    }
    
    protected override pb::FieldAccess.FieldAccessorTable<normal, normal.Builder> InternalFieldAccessors {
      get { return global::ProtoCmd.BackRecharge.BackRecharge.internal__static_ProtoCmd_BackRecharge_normal__FieldAccessorTable; }
    }
    
    public const int UserIDFieldNumber = 1;
    private bool hasUserID;
    private uint userID_;
    public bool HasUserID {
      get { return hasUserID; }
    }
    [global::System.CLSCompliant(false)]
    public uint UserID {
      get { return userID_; }
    }
    
    public const int ListFieldNumber = 2;
    private bool hasList;
    private string list_ = "";
    public bool HasList {
      get { return hasList; }
    }
    public string List {
      get { return list_; }
    }
    
    public const int RmbFieldNumber = 3;
    private bool hasRmb;
    private uint rmb_;
    public bool HasRmb {
      get { return hasRmb; }
    }
    [global::System.CLSCompliant(false)]
    public uint Rmb {
      get { return rmb_; }
    }
    
    public const int FirstGifFieldNumber = 4;
    private bool hasFirstGif;
    private bool firstGif_;
    public bool HasFirstGif {
      get { return hasFirstGif; }
    }
    public bool FirstGif {
      get { return firstGif_; }
    }
    
    public const int BillNoFieldNumber = 5;
    private bool hasBillNo;
    private string billNo_ = "";
    public bool HasBillNo {
      get { return hasBillNo; }
    }
    public string BillNo {
      get { return billNo_; }
    }
    
    public const int RmbActualFieldNumber = 6;
    private bool hasRmbActual;
    private uint rmbActual_;
    public bool HasRmbActual {
      get { return hasRmbActual; }
    }
    [global::System.CLSCompliant(false)]
    public uint RmbActual {
      get { return rmbActual_; }
    }
    
    public override bool IsInitialized {
      get {
        if (!hasUserID) return false;
        if (!hasList) return false;
        if (!hasRmb) return false;
        if (!hasFirstGif) return false;
        return true;
      }
    }
    
    public override void WriteTo(pb::ICodedOutputStream output) {
      CalcSerializedSize();
      string[] field_names = _normalFieldNames;
      if (hasUserID) {
        output.WriteUInt32(1, field_names[5], UserID);
      }
      if (hasList) {
        output.WriteString(2, field_names[2], List);
      }
      if (hasRmb) {
        output.WriteUInt32(3, field_names[3], Rmb);
      }
      if (hasFirstGif) {
        output.WriteBool(4, field_names[1], FirstGif);
      }
      if (hasBillNo) {
        output.WriteString(5, field_names[0], BillNo);
      }
      if (hasRmbActual) {
        output.WriteUInt32(6, field_names[4], RmbActual);
      }
      UnknownFields.WriteTo(output);
    }
    
    private int memoizedSerializedSize = -1;
    public override int SerializedSize {
      get {
        int size = memoizedSerializedSize;
        if (size != -1) return size;
        return CalcSerializedSize();
      }
    }
    
    private int CalcSerializedSize() {
      int size = memoizedSerializedSize;
      if (size != -1) return size;
      
      size = 0;
      if (hasUserID) {
        size += pb::CodedOutputStream.ComputeUInt32Size(1, UserID);
      }
      if (hasList) {
        size += pb::CodedOutputStream.ComputeStringSize(2, List);
      }
      if (hasRmb) {
        size += pb::CodedOutputStream.ComputeUInt32Size(3, Rmb);
      }
      if (hasFirstGif) {
        size += pb::CodedOutputStream.ComputeBoolSize(4, FirstGif);
      }
      if (hasBillNo) {
        size += pb::CodedOutputStream.ComputeStringSize(5, BillNo);
      }
      if (hasRmbActual) {
        size += pb::CodedOutputStream.ComputeUInt32Size(6, RmbActual);
      }
      size += UnknownFields.SerializedSize;
      memoizedSerializedSize = size;
      return size;
    }
    public static normal ParseFrom(pb::ByteString data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static normal ParseFrom(pb::ByteString data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static normal ParseFrom(byte[] data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static normal ParseFrom(byte[] data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static normal ParseFrom(global::System.IO.Stream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static normal ParseFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    public static normal ParseDelimitedFrom(global::System.IO.Stream input) {
      return CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
    }
    public static normal ParseDelimitedFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
    }
    public static normal ParseFrom(pb::ICodedInputStream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static normal ParseFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    private normal MakeReadOnly() {
      return this;
    }
    
    public static Builder CreateBuilder() { return new Builder(); }
    public override Builder ToBuilder() { return CreateBuilder(this); }
    public override Builder CreateBuilderForType() { return new Builder(); }
    public static Builder CreateBuilder(normal prototype) {
      return new Builder(prototype);
    }
    
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public sealed partial class Builder : pb::GeneratedBuilder<normal, Builder> {
      protected override Builder ThisBuilder {
        get { return this; }
      }
      public Builder() {
        result = DefaultInstance;
        resultIsReadOnly = true;
      }
      internal Builder(normal cloneFrom) {
        result = cloneFrom;
        resultIsReadOnly = true;
      }
      
      private bool resultIsReadOnly;
      private normal result;
      
      private normal PrepareBuilder() {
        if (resultIsReadOnly) {
          normal original = result;
          result = new normal();
          resultIsReadOnly = false;
          MergeFrom(original);
        }
        return result;
      }
      
      public override bool IsInitialized {
        get { return result.IsInitialized; }
      }
      
      protected override normal MessageBeingBuilt {
        get { return PrepareBuilder(); }
      }
      
      public override Builder Clear() {
        result = DefaultInstance;
        resultIsReadOnly = true;
        return this;
      }
      
      public override Builder Clone() {
        if (resultIsReadOnly) {
          return new Builder(result);
        } else {
          return new Builder().MergeFrom(result);
        }
      }
      
      public override pbd::MessageDescriptor DescriptorForType {
        get { return global::ProtoCmd.BackRecharge.normal.Descriptor; }
      }
      
      public override normal DefaultInstanceForType {
        get { return global::ProtoCmd.BackRecharge.normal.DefaultInstance; }
      }
      
      public override normal BuildPartial() {
        if (resultIsReadOnly) {
          return result;
        }
        resultIsReadOnly = true;
        return result.MakeReadOnly();
      }
      
      public override Builder MergeFrom(pb::IMessage other) {
        if (other is normal) {
          return MergeFrom((normal) other);
        } else {
          base.MergeFrom(other);
          return this;
        }
      }
      
      public override Builder MergeFrom(normal other) {
        if (other == global::ProtoCmd.BackRecharge.normal.DefaultInstance) return this;
        PrepareBuilder();
        if (other.HasUserID) {
          UserID = other.UserID;
        }
        if (other.HasList) {
          List = other.List;
        }
        if (other.HasRmb) {
          Rmb = other.Rmb;
        }
        if (other.HasFirstGif) {
          FirstGif = other.FirstGif;
        }
        if (other.HasBillNo) {
          BillNo = other.BillNo;
        }
        if (other.HasRmbActual) {
          RmbActual = other.RmbActual;
        }
        this.MergeUnknownFields(other.UnknownFields);
        return this;
      }
      
      public override Builder MergeFrom(pb::ICodedInputStream input) {
        return MergeFrom(input, pb::ExtensionRegistry.Empty);
      }
      
      public override Builder MergeFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
        PrepareBuilder();
        pb::UnknownFieldSet.Builder unknownFields = null;
        uint tag;
        string field_name;
        while (input.ReadTag(out tag, out field_name)) {
          if(tag == 0 && field_name != null) {
            int field_ordinal = global::System.Array.BinarySearch(_normalFieldNames, field_name, global::System.StringComparer.Ordinal);
            if(field_ordinal >= 0)
              tag = _normalFieldTags[field_ordinal];
            else {
              if (unknownFields == null) {
                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
              }
              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
              continue;
            }
          }
          switch (tag) {
            case 0: {
              throw pb::InvalidProtocolBufferException.InvalidTag();
            }
            default: {
              if (pb::WireFormat.IsEndGroupTag(tag)) {
                if (unknownFields != null) {
                  this.UnknownFields = unknownFields.Build();
                }
                return this;
              }
              if (unknownFields == null) {
                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
              }
              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
              break;
            }
            case 8: {
              result.hasUserID = input.ReadUInt32(ref result.userID_);
              break;
            }
            case 18: {
              result.hasList = input.ReadString(ref result.list_);
              break;
            }
            case 24: {
              result.hasRmb = input.ReadUInt32(ref result.rmb_);
              break;
            }
            case 32: {
              result.hasFirstGif = input.ReadBool(ref result.firstGif_);
              break;
            }
            case 42: {
              result.hasBillNo = input.ReadString(ref result.billNo_);
              break;
            }
            case 48: {
              result.hasRmbActual = input.ReadUInt32(ref result.rmbActual_);
              break;
            }
          }
        }
        
        if (unknownFields != null) {
          this.UnknownFields = unknownFields.Build();
        }
        return this;
      }
      
      
      public bool HasUserID {
        get { return result.hasUserID; }
      }
      [global::System.CLSCompliant(false)]
      public uint UserID {
        get { return result.UserID; }
        set { SetUserID(value); }
      }
      [global::System.CLSCompliant(false)]
      public Builder SetUserID(uint value) {
        PrepareBuilder();
        result.hasUserID = true;
        result.userID_ = value;
        return this;
      }
      public Builder ClearUserID() {
        PrepareBuilder();
        result.hasUserID = false;
        result.userID_ = 0;
        return this;
      }
      
      public bool HasList {
        get { return result.hasList; }
      }
      public string List {
        get { return result.List; }
        set { SetList(value); }
      }
      public Builder SetList(string value) {
        pb::ThrowHelper.ThrowIfNull(value, "value");
        PrepareBuilder();
        result.hasList = true;
        result.list_ = value;
        return this;
      }
      public Builder ClearList() {
        PrepareBuilder();
        result.hasList = false;
        result.list_ = "";
        return this;
      }
      
      public bool HasRmb {
        get { return result.hasRmb; }
      }
      [global::System.CLSCompliant(false)]
      public uint Rmb {
        get { return result.Rmb; }
        set { SetRmb(value); }
      }
      [global::System.CLSCompliant(false)]
      public Builder SetRmb(uint value) {
        PrepareBuilder();
        result.hasRmb = true;
        result.rmb_ = value;
        return this;
      }
      public Builder ClearRmb() {
        PrepareBuilder();
        result.hasRmb = false;
        result.rmb_ = 0;
        return this;
      }
      
      public bool HasFirstGif {
        get { return result.hasFirstGif; }
      }
      public bool FirstGif {
        get { return result.FirstGif; }
        set { SetFirstGif(value); }
      }
      public Builder SetFirstGif(bool value) {
        PrepareBuilder();
        result.hasFirstGif = true;
        result.firstGif_ = value;
        return this;
      }
      public Builder ClearFirstGif() {
        PrepareBuilder();
        result.hasFirstGif = false;
        result.firstGif_ = false;
        return this;
      }
      
      public bool HasBillNo {
        get { return result.hasBillNo; }
      }
      public string BillNo {
        get { return result.BillNo; }
        set { SetBillNo(value); }
      }
      public Builder SetBillNo(string value) {
        pb::ThrowHelper.ThrowIfNull(value, "value");
        PrepareBuilder();
        result.hasBillNo = true;
        result.billNo_ = value;
        return this;
      }
      public Builder ClearBillNo() {
        PrepareBuilder();
        result.hasBillNo = false;
        result.billNo_ = "";
        return this;
      }
      
      public bool HasRmbActual {
        get { return result.hasRmbActual; }
      }
      [global::System.CLSCompliant(false)]
      public uint RmbActual {
        get { return result.RmbActual; }
        set { SetRmbActual(value); }
      }
      [global::System.CLSCompliant(false)]
      public Builder SetRmbActual(uint value) {
        PrepareBuilder();
        result.hasRmbActual = true;
        result.rmbActual_ = value;
        return this;
      }
      public Builder ClearRmbActual() {
        PrepareBuilder();
        result.hasRmbActual = false;
        result.rmbActual_ = 0;
        return this;
      }
    }
    static normal() {
      object.ReferenceEquals(global::ProtoCmd.BackRecharge.BackRecharge.Descriptor, null);
    }
  }
  
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public sealed partial class normalRep : pb::GeneratedMessage<normalRep, normalRep.Builder> {
    private normalRep() { }
    private static readonly normalRep defaultInstance = new normalRep().MakeReadOnly();
    private static readonly string[] _normalRepFieldNames = new string[] { "suc", "userID" };
    private static readonly uint[] _normalRepFieldTags = new uint[] { 16, 8 };
    public static normalRep DefaultInstance {
      get { return defaultInstance; }
    }
    
    public override normalRep DefaultInstanceForType {
      get { return DefaultInstance; }
    }
    
    protected override normalRep ThisMessage {
      get { return this; }
    }
    
    public static pbd::MessageDescriptor Descriptor {
      get { return global::ProtoCmd.BackRecharge.BackRecharge.internal__static_ProtoCmd_BackRecharge_normalRep__Descriptor; }
    }
    
    protected override pb::FieldAccess.FieldAccessorTable<normalRep, normalRep.Builder> InternalFieldAccessors {
      get { return global::ProtoCmd.BackRecharge.BackRecharge.internal__static_ProtoCmd_BackRecharge_normalRep__FieldAccessorTable; }
    }
    
    public const int UserIDFieldNumber = 1;
    private bool hasUserID;
    private uint userID_;
    public bool HasUserID {
      get { return hasUserID; }
    }
    [global::System.CLSCompliant(false)]
    public uint UserID {
      get { return userID_; }
    }
    
    public const int SucFieldNumber = 2;
    private bool hasSuc;
    private bool suc_;
    public bool HasSuc {
      get { return hasSuc; }
    }
    public bool Suc {
      get { return suc_; }
    }
    
    public override bool IsInitialized {
      get {
        if (!hasUserID) return false;
        if (!hasSuc) return false;
        return true;
      }
    }
    
    public override void WriteTo(pb::ICodedOutputStream output) {
      CalcSerializedSize();
      string[] field_names = _normalRepFieldNames;
      if (hasUserID) {
        output.WriteUInt32(1, field_names[1], UserID);
      }
      if (hasSuc) {
        output.WriteBool(2, field_names[0], Suc);
      }
      UnknownFields.WriteTo(output);
    }
    
    private int memoizedSerializedSize = -1;
    public override int SerializedSize {
      get {
        int size = memoizedSerializedSize;
        if (size != -1) return size;
        return CalcSerializedSize();
      }
    }
    
    private int CalcSerializedSize() {
      int size = memoizedSerializedSize;
      if (size != -1) return size;
      
      size = 0;
      if (hasUserID) {
        size += pb::CodedOutputStream.ComputeUInt32Size(1, UserID);
      }
      if (hasSuc) {
        size += pb::CodedOutputStream.ComputeBoolSize(2, Suc);
      }
      size += UnknownFields.SerializedSize;
      memoizedSerializedSize = size;
      return size;
    }
    public static normalRep ParseFrom(pb::ByteString data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static normalRep ParseFrom(pb::ByteString data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static normalRep ParseFrom(byte[] data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static normalRep ParseFrom(byte[] data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static normalRep ParseFrom(global::System.IO.Stream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static normalRep ParseFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    public static normalRep ParseDelimitedFrom(global::System.IO.Stream input) {
      return CreateBuilder().MergeDelimitedFrom(input).BuildParsed();
    }
    public static normalRep ParseDelimitedFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return CreateBuilder().MergeDelimitedFrom(input, extensionRegistry).BuildParsed();
    }
    public static normalRep ParseFrom(pb::ICodedInputStream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static normalRep ParseFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    private normalRep MakeReadOnly() {
      return this;
    }
    
    public static Builder CreateBuilder() { return new Builder(); }
    public override Builder ToBuilder() { return CreateBuilder(this); }
    public override Builder CreateBuilderForType() { return new Builder(); }
    public static Builder CreateBuilder(normalRep prototype) {
      return new Builder(prototype);
    }
    
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public sealed partial class Builder : pb::GeneratedBuilder<normalRep, Builder> {
      protected override Builder ThisBuilder {
        get { return this; }
      }
      public Builder() {
        result = DefaultInstance;
        resultIsReadOnly = true;
      }
      internal Builder(normalRep cloneFrom) {
        result = cloneFrom;
        resultIsReadOnly = true;
      }
      
      private bool resultIsReadOnly;
      private normalRep result;
      
      private normalRep PrepareBuilder() {
        if (resultIsReadOnly) {
          normalRep original = result;
          result = new normalRep();
          resultIsReadOnly = false;
          MergeFrom(original);
        }
        return result;
      }
      
      public override bool IsInitialized {
        get { return result.IsInitialized; }
      }
      
      protected override normalRep MessageBeingBuilt {
        get { return PrepareBuilder(); }
      }
      
      public override Builder Clear() {
        result = DefaultInstance;
        resultIsReadOnly = true;
        return this;
      }
      
      public override Builder Clone() {
        if (resultIsReadOnly) {
          return new Builder(result);
        } else {
          return new Builder().MergeFrom(result);
        }
      }
      
      public override pbd::MessageDescriptor DescriptorForType {
        get { return global::ProtoCmd.BackRecharge.normalRep.Descriptor; }
      }
      
      public override normalRep DefaultInstanceForType {
        get { return global::ProtoCmd.BackRecharge.normalRep.DefaultInstance; }
      }
      
      public override normalRep BuildPartial() {
        if (resultIsReadOnly) {
          return result;
        }
        resultIsReadOnly = true;
        return result.MakeReadOnly();
      }
      
      public override Builder MergeFrom(pb::IMessage other) {
        if (other is normalRep) {
          return MergeFrom((normalRep) other);
        } else {
          base.MergeFrom(other);
          return this;
        }
      }
      
      public override Builder MergeFrom(normalRep other) {
        if (other == global::ProtoCmd.BackRecharge.normalRep.DefaultInstance) return this;
        PrepareBuilder();
        if (other.HasUserID) {
          UserID = other.UserID;
        }
        if (other.HasSuc) {
          Suc = other.Suc;
        }
        this.MergeUnknownFields(other.UnknownFields);
        return this;
      }
      
      public override Builder MergeFrom(pb::ICodedInputStream input) {
        return MergeFrom(input, pb::ExtensionRegistry.Empty);
      }
      
      public override Builder MergeFrom(pb::ICodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
        PrepareBuilder();
        pb::UnknownFieldSet.Builder unknownFields = null;
        uint tag;
        string field_name;
        while (input.ReadTag(out tag, out field_name)) {
          if(tag == 0 && field_name != null) {
            int field_ordinal = global::System.Array.BinarySearch(_normalRepFieldNames, field_name, global::System.StringComparer.Ordinal);
            if(field_ordinal >= 0)
              tag = _normalRepFieldTags[field_ordinal];
            else {
              if (unknownFields == null) {
                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
              }
              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
              continue;
            }
          }
          switch (tag) {
            case 0: {
              throw pb::InvalidProtocolBufferException.InvalidTag();
            }
            default: {
              if (pb::WireFormat.IsEndGroupTag(tag)) {
                if (unknownFields != null) {
                  this.UnknownFields = unknownFields.Build();
                }
                return this;
              }
              if (unknownFields == null) {
                unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
              }
              ParseUnknownField(input, unknownFields, extensionRegistry, tag, field_name);
              break;
            }
            case 8: {
              result.hasUserID = input.ReadUInt32(ref result.userID_);
              break;
            }
            case 16: {
              result.hasSuc = input.ReadBool(ref result.suc_);
              break;
            }
          }
        }
        
        if (unknownFields != null) {
          this.UnknownFields = unknownFields.Build();
        }
        return this;
      }
      
      
      public bool HasUserID {
        get { return result.hasUserID; }
      }
      [global::System.CLSCompliant(false)]
      public uint UserID {
        get { return result.UserID; }
        set { SetUserID(value); }
      }
      [global::System.CLSCompliant(false)]
      public Builder SetUserID(uint value) {
        PrepareBuilder();
        result.hasUserID = true;
        result.userID_ = value;
        return this;
      }
      public Builder ClearUserID() {
        PrepareBuilder();
        result.hasUserID = false;
        result.userID_ = 0;
        return this;
      }
      
      public bool HasSuc {
        get { return result.hasSuc; }
      }
      public bool Suc {
        get { return result.Suc; }
        set { SetSuc(value); }
      }
      public Builder SetSuc(bool value) {
        PrepareBuilder();
        result.hasSuc = true;
        result.suc_ = value;
        return this;
      }
      public Builder ClearSuc() {
        PrepareBuilder();
        result.hasSuc = false;
        result.suc_ = false;
        return this;
      }
    }
    static normalRep() {
      object.ReferenceEquals(global::ProtoCmd.BackRecharge.BackRecharge.Descriptor, null);
    }
  }
  
  #endregion
  
}

#endregion Designer generated code