using System;
using System.Drawing;
using System.ComponentModel;
using FastReport.TypeConverters;
using FastReport.Utils;
using System.Reflection;

namespace FastReport
{
   /// <summary>
   /// Specifies the Save Mode of designed report.
   /// </summary>
   public enum SaveMode
   {
      /// <summary>
      /// The saving allowed to all.
      /// </summary>
      All = 0,
      /// <summary>
      /// The saving in original place.
      /// </summary>
      Original,
      /// <summary>
      /// The saving allowed to current user.
      /// </summary>
      User,
      /// <summary>
      /// The saving allowed to current role/group.
      /// </summary>
      Role,
      /// <summary>
      /// The saving allowed with other security permissions.
      /// </summary>
      Security,
      /// <summary>
      /// The saving not allowed.
      /// </summary>
      Deny,
      /// <summary>
      /// Custom saving rules.
      /// </summary>
      Custom 
   }

  /// <summary>
  /// This class represents the report information such as name, author, description etc.
  /// </summary>
  [TypeConverter(typeof(FRExpandableObjectConverter))]
  public class ReportInfo
  {
    #region Fields
    private string FName;
    private string FAuthor;
    private string FVersion;
    private string FDescription;
    private Image FPicture;
    private DateTime FCreated;
    private DateTime FModified;
    private bool FSavePreviewPicture;
    private float FPreviewPictureRatio;
    private string FCreatorVersion;
    private string FTag;
    private SaveMode FSaveMode;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the name of a report.
    /// </summary>
    public string Name
    {
      get { return FName; }
      set { FName = value; }
    }

    /// <summary>
    /// Gets or sets the author of a report.
    /// </summary>
    public string Author
    {
      get { return FAuthor; }
      set { FAuthor = value; }
    }

    /// <summary>
    /// Gets or sets the report version.
    /// </summary>
    public string Version
    {
      get { return FVersion; }
      set { FVersion = value; }
    }

    /// <summary>
    /// Gets or sets the report description.
    /// </summary>
    [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
    public string Description
    {
      get { return FDescription; }
      set { FDescription = value; }
    }

    /// <summary>
    /// Gets or sets the picture associated with a report.
    /// </summary>
    public Image Picture
    {
      get { return FPicture; }
      set { FPicture = value; }
    }

    /// <summary>
    /// Gets or sets the report creation date and time.
    /// </summary>
    public DateTime Created
    {
      get { return FCreated; }
      set { FCreated = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating that report was modified in the designer.
    /// </summary>
    public DateTime Modified
    {
      get { return FModified; }
      set { FModified = value; }
    }

    /// <summary>
    /// Gets or sets a value that determines whether to fill the <see cref="Picture"/> property
    /// automatically.
    /// </summary>
    [DefaultValue(false)]
    public bool SavePreviewPicture
    {
      get { return FSavePreviewPicture; }
      set { FSavePreviewPicture = value; }
    }

    /// <summary>
    /// Gets or sets the ratio that will be used when generating a preview picture.
    /// </summary>
    [DefaultValue(0.1f)]
    public float PreviewPictureRatio
    {
      get { return FPreviewPictureRatio; }
      set 
      {
        if (value <= 0)
          value = 0.05f;
        FPreviewPictureRatio = value; 
      }
    }
    
    /// <summary>
    /// Gets the version of FastReport that was created this report file.
    /// </summary>
    public string CreatorVersion
    {
      get { return FCreatorVersion; }
      set { FCreatorVersion = value; }
    }

    /// <summary>
    /// Gets or sets the Tag string object for this report file.
    /// </summary>
    [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
    public string Tag
    {
      get { return FTag; }
      set { FTag = value; }
    }

    /// <summary>
    /// Gets or sets SaveMode property.
    /// </summary>
    [DefaultValue(SaveMode.All)]
    public SaveMode SaveMode
    {
      get { return FSaveMode; }
      set { FSaveMode = value; }
    }
    
    private string CurrentVersion
    {
      get 
      {
        AssemblyName asm = new AssemblyName(GetType().Assembly.FullName);
        return asm.Version.ToString(); 
      }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Resets all properties to its default values.
    /// </summary>
    public void Clear()
    {
      FName = "";
      FAuthor = "";
      FVersion = "";
      FDescription = "";
      FTag = "";
      if (FPicture != null)
        FPicture.Dispose();
      FPicture = null;
      FCreated = DateTime.Now;
      FModified = DateTime.Now;
      FSavePreviewPicture = false;
      FPreviewPictureRatio = 0.1f;
      FCreatorVersion = CurrentVersion;
      FSaveMode = SaveMode.All;
    }
    
    internal void Serialize(FRWriter writer, ReportInfo c)
    {
      if (Name != c.Name)
        writer.WriteStr("ReportInfo.Name", Name);
      if (Author != c.Author)
        writer.WriteStr("ReportInfo.Author", Author);
      if (Version != c.Version)
        writer.WriteStr("ReportInfo.Version", Version);
      if (Description != c.Description)
        writer.WriteStr("ReportInfo.Description", Description);
      if (Tag != c.Tag)
        writer.WriteStr("ReportInfo.Tag", Tag);
      if (!writer.AreEqual(Picture, c.Picture))
        writer.WriteValue("ReportInfo.Picture", Picture);
      writer.WriteValue("ReportInfo.Created", Created);
      FModified = DateTime.Now;
      writer.WriteValue("ReportInfo.Modified", Modified);
      if (SavePreviewPicture != c.SavePreviewPicture)
        writer.WriteBool("ReportInfo.SavePreviewPicture", SavePreviewPicture);
      if (PreviewPictureRatio != c.PreviewPictureRatio)
        writer.WriteFloat("ReportInfo.PreviewPictureRatio", PreviewPictureRatio);
      writer.WriteStr("ReportInfo.CreatorVersion", CurrentVersion);
      if (SaveMode != c.SaveMode)
        writer.WriteValue("ReportInfo.SaveMode", SaveMode);
    }
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportInfo"/> class with default settings.
    /// </summary>
    public ReportInfo() 
    {
      Clear();
    }
  }
}