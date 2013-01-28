// Copyright (c) 2007 Gratian Lup. All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
// * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials provided
// with the distribution.
//
// * The name "SecureDelete" must not be used to endorse or promote
// products derived from this software without prior written permission.
//
// * Products derived from this software may not be called "SecureDelete" nor
// may "SecureDelete" appear in their names without prior written
// permission of the author.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using DebugUtils.Debugger;
using System.IO;

namespace SecureDelete {
    class ReportExporter {
        public static bool ExportAsText(WipeReport report, string path) {
            StreamWriter writer = null;

            try {
                writer = new StreamWriter(path);

                if(writer.BaseStream.CanWrite == false) {
                    Debug.ReportWarning("Cannot write to file {0}", path);
                    return false;
                }

                // header
                writer.WriteLine("SecureDelete Wipe Report\r\n");

                // statistics
                if(report.Statistics != null) {
                    writer.WriteLine("Statistics");
                    writer.WriteLine("------------------------\r\n");

                    writer.Write("Start Time: ");
                    writer.WriteLine(report.Statistics.StartTime.ToString());
                    writer.Write("End Time: ");
                    writer.WriteLine(report.Statistics.EndTime.ToString());
                    writer.Write("Duration: ");
                    writer.WriteLine(report.Statistics.Duration.ToString());
                    writer.Write("Failed Objects: ");
                    writer.WriteLine(report.Statistics.FailedObjects);
                    writer.Write("Errors: ");
                    writer.WriteLine(report.Statistics.Errors);
                    writer.Write("Average Write Speed: ");
                    writer.WriteLine(report.Statistics.AverageWriteSpeed);
                    writer.Write("Total Wiped Bytes: ");
                    writer.WriteLine(report.Statistics.TotalWipedBytes);
                    writer.Write("Bytes In Cluster Tips: ");
                    writer.WriteLine(report.Statistics.BytesInClusterTips);

                    writer.Write("\r\n");
                }

                // failed objects
                writer.WriteLine("Failed Objects - " + report.FailedObjects.Count.ToString());
                writer.WriteLine("------------------------");

                for(int i = 0; i < report.FailedObjects.Count; i++) {
                    writer.WriteLine("\r\n#{0}", i);
                    writer.Write("Type: ");
                    writer.WriteLine(report.FailedObjects[i].Type.ToString());
                    writer.Write("Path: ");
                    writer.WriteLine(report.FailedObjects[i].Path);
                }

                // errors
                writer.WriteLine("Errors - " + report.Errors.Count.ToString());
                writer.WriteLine("------------------------");

                for(int i = 0; i < report.Errors.Count; i++) {
                    writer.WriteLine("\r\n#{0}", i);
                    writer.Write("Time: ");
                    writer.WriteLine(report.Errors[i].Time.ToString());
                    writer.Write("Severity: ");
                    writer.WriteLine(report.Errors[i].Severity.ToString());
                    writer.Write("Message: ");
                    writer.WriteLine(report.Errors[i].Message);
                }

                return true;
            }
            catch(Exception e) {
                Debug.ReportWarning("Cannot write to file {0}. Exception: {0}", e.Message);
                return false;
            }
            finally {
                if(writer != null && writer.BaseStream.CanWrite) {
                    writer.Close();
                }
            }
        }


        public static bool ExportAsXml(WipeReport report, string path) {
            StreamWriter writer = null;

            try {
                writer = new StreamWriter(path);

                if(writer.BaseStream.CanWrite == false) {
                    Debug.ReportWarning("Cannot write to file {0}", path);
                    return false;
                }

                // write xml file structure
                writer.WriteLine("<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>");
                writer.WriteLine("<WipeReport>");

                // basic info
                if(report.Statistics != null) {
                    writer.WriteLine("<Statistics>");
                    writer.Write("<StartTime>");
                    writer.Write(report.Statistics.StartTime.ToString());
                    writer.WriteLine("</StartTime>");
                    writer.Write("<EndTime>");
                    writer.Write(report.Statistics.EndTime.ToString());
                    writer.WriteLine("</EndTime>");
                    writer.Write("<Duration>");
                    writer.Write(report.Statistics.Duration.ToString());
                    writer.WriteLine("</Duration>");
                    writer.Write("<FailedObjects>");
                    writer.Write(report.Statistics.FailedObjects);
                    writer.WriteLine("</FailedObjects>");
                    writer.Write("<Errors>");
                    writer.Write(report.Statistics.Errors);
                    writer.WriteLine("</Errors>");
                    writer.Write("<AverageWriteSpeed>");
                    writer.Write(report.Statistics.AverageWriteSpeed);
                    writer.WriteLine("</AverageWriteSpeed>");
                    writer.Write("<TotalWipedBytes>");
                    writer.Write(report.Statistics.TotalWipedBytes);
                    writer.WriteLine("</TotalWipedBytes>");
                    writer.Write("<BytesInClusterTips>");
                    writer.Write(report.Statistics.BytesInClusterTips);
                    writer.WriteLine("</BytesInClusterTips>");
                    writer.WriteLine("</Statistics>");
                }

                // failed objects
                writer.WriteLine("<FailedObjects>");
                for(int i = 0; i < report.FailedObjects.Count; i++) {
                    writer.WriteLine("<Object>");
                    writer.Write("<Type>");
                    writer.Write(report.FailedObjects[i].Type.ToString());
                    writer.WriteLine("</Type>");
                    writer.Write("<Path>");
                    writer.Write(report.FailedObjects[i].Path);
                    writer.WriteLine("</Path>");
                    writer.WriteLine("</Object>");
                }
                writer.WriteLine("</FailedObjects>");

                // errors
                writer.WriteLine("<Errors>");
                for(int i = 0; i < report.Errors.Count; i++) {
                    writer.WriteLine("<Error>");
                    writer.Write("<Time>");
                    writer.Write(report.Errors[i].Time.ToString());
                    writer.WriteLine("</Time>");
                    writer.Write("<Severity>");
                    writer.Write(report.Errors[i].Severity.ToString());
                    writer.WriteLine("</Severity>");
                    writer.Write("<Message>");
                    writer.Write(report.Errors[i].Message);
                    writer.WriteLine("</Message>");
                    writer.WriteLine("</Error>");
                }
                writer.WriteLine("</Errors>");

                // end of document
                writer.WriteLine("</WipeReport>");
                return true;
            }
            catch(Exception e) {
                Debug.ReportWarning("Cannot write to file {0}. Exception: {0}", e.Message);
                return false;
            }
            finally {
                if(writer != null && writer.BaseStream.CanWrite) {
                    writer.Close();
                }
            }
        }


        public static bool ExportAsHtml(WipeReport report, string path, string style) {
            StreamWriter writer = null;

            try {
                writer = new StreamWriter(path);

                if(writer.BaseStream.CanWrite == false) {
                    Debug.ReportWarning("Cannot write to file {0}", path);
                    return false;
                }

                writer.Write(GetHtmlString(report, style));
                return true;
            }
            catch(Exception e) {
                Debug.ReportWarning("Cannot write to file {0}. Exception: {0}", e.Message);
                return false;
            }
            finally {
                if(writer != null && writer.BaseStream.CanWrite) {
                    writer.Close();
                }
            }
        }


        public static string GetHtmlString(WipeReport report, string style) {
            StringBuilder builder = new StringBuilder();

            // write html file structure
            builder.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            builder.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">");
            builder.AppendLine("<head>");
            builder.AppendLine("<meta http-equiv=\"Content-Language\" content=\"en-us\" />");
            builder.AppendLine("<meta http-equiv=\"Content-Type\\ content=\"text/html; charset=utf-8\" />");
            builder.AppendLine("<title>Wipe Report</title>");

            // load styles from resource
            builder.AppendLine("<style type=\"text/css\">");
            builder.AppendLine(style);
            builder.AppendLine("</style>");

            builder.AppendLine("</head>");
            builder.AppendLine("<body class=\"BodyStyle\">");
            builder.AppendLine("<p name=\"#top\" class=\"Header\">Wipe Report</p>");
            builder.Append("<p class=\"Subheader\">Report created on " + DateTime.Now.ToString());
            builder.AppendLine("</p>");

            // write statistics
            if(report.Statistics != null) {
                builder.AppendLine("<p class=\"Subtitle\">Statistics</p>");
                builder.AppendLine("<p class=\"Normal\">");

                builder.Append("Start Time: ");
                builder.Append(report.Statistics.StartTime.ToString());
                builder.AppendLine("<br/>");
                builder.Append("End Time: ");
                builder.Append(report.Statistics.EndTime.ToString());
                builder.AppendLine("<br/>");
                builder.Append("Duration: ");
                builder.Append(report.Statistics.Duration.ToString());
                builder.AppendLine("<br/>");
                builder.Append("Failed Objects: ");
                builder.Append(report.Statistics.FailedObjects);
                builder.AppendLine("<br/>");
                builder.Append("Errors: ");
                builder.Append(report.Statistics.Errors);
                builder.AppendLine("<br/>");
                builder.Append("Average Write Speed: ");
                builder.Append(report.Statistics.AverageWriteSpeed);
                builder.AppendLine("<br/>");
                builder.Append("Total Wiped Bytes: ");
                builder.Append(report.Statistics.TotalWipedBytes);
                builder.AppendLine("<br/>");
                builder.Append("Bytes In Cluster Tips: ");
                builder.Append(report.Statistics.BytesInClusterTips);
                builder.AppendLine("<br/>");

                builder.AppendLine("</p>");
            }

            // failed objects
            builder.AppendLine("<p class=\"Subtitle\">" + report.FailedObjects.Count.ToString() + " Failed objects</p>");

            if(report.FailedObjects.Count > 0) {
                // create table
                builder.AppendLine("<table style=\"width: 100%\" class=\"TableStyle\"");

                // create columns
                builder.AppendLine("<tr>");
                builder.AppendLine("<td class=\"TabelHeader2\" style=\"width: 50px\">ID</td>");
                builder.AppendLine("<td class=\"TabelHeader\" style=\"width: 120px\">Type</td>");
                builder.AppendLine("<td class=\"TabelHeader\">Path</td>");
                builder.AppendLine("<td class=\"TabelHeader2\" style=\"width: 160px\">Associated error</td>");
                builder.AppendLine("</tr>");

                // write failed objects
                for(int i = 0; i < report.FailedObjects.Count; i++) {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td class=\"Normal2\">" + ((int)(i + 1)).ToString() + "</td>");
                    builder.AppendLine("<td class=\"Normal\">" + report.FailedObjects[i].Type.ToString() + "</td>");
                    builder.AppendLine("<td class=\"Normal\">" + report.FailedObjects[i].Path + "</td>");

                    // associated error <a href="#test">
                    if(report.FailedObjects[i].AssociatedError != null) {
                        // find the indes
                        int index = report.Errors.IndexOf(report.FailedObjects[i].AssociatedError) + 1;
                        builder.AppendLine("<td class=\"Normal2\"><a href=\"#" + index.ToString() + "\"> #" + index.ToString() + "</a></td>");
                    }
                    else {
                        builder.AppendLine("<td class=\"Normal\">-</td>");
                    }

                    builder.AppendLine("</tr>");
                }

                builder.AppendLine("</table>");
                builder.AppendLine("<p class=\"NavigatorStyle\"><a class=\"NavigatorStyle\" href=\"#top\">Top</a></p>");
            }

            // errors
            builder.AppendLine("<p class=\"Subtitle\">" + report.Errors.Count.ToString() + " Errors</p>");

            if(report.Errors.Count > 0) {
                // create table
                builder.AppendLine("<table style=\"width: 100%\" class=\"TableStyle\"");

                // create columns
                builder.AppendLine("<tr>");
                builder.AppendLine("<td class=\"TabelHeader2\" style=\"width: 50px\">ID</td>");
                builder.AppendLine("<td class=\"TabelHeader\" style=\"width: 120px\">Time</td>");
                builder.AppendLine("<td class=\"TabelHeader\" style=\"width: 120px\">Severity</td>");
                builder.AppendLine("<td class=\"TabelHeader\">Message</td>");
                builder.AppendLine("</tr>");

                // write failed objects
                for(int i = 0; i < report.Errors.Count; i++) {
                    builder.AppendLine("<tr>");
                    builder.Append("<td class=\"");

                    switch(report.Errors[i].Severity) {
                        case ErrorSeverity.High: {
                            builder.Append("HighSeverity");
                            break;
                        }
                        case ErrorSeverity.Medium: {
                            builder.Append("MediumSeverity");
                            break;
                        }
                        case ErrorSeverity.Low: {
                            builder.Append("LowSeverity");
                            break;
                        }
                    }

                    builder.Append("\">");
                    int number = i + 1;
                    builder.Append("<a name=\"#" + number.ToString() + "\">");
                    builder.AppendLine(number.ToString() + "</a></td>");
                    builder.AppendLine("<td class=\"Normal\">" + report.Errors[i].Time.ToLongTimeString() + "</td>");
                    builder.AppendLine("<td class=\"Normal\">" + report.Errors[i].Severity.ToString() + "</td>");
                    builder.AppendLine("<td class=\"Normal\">" + report.Errors[i].Message.ToString() + "</td>");
                    builder.AppendLine("</tr>");
                }

                builder.AppendLine("</table>");
                builder.AppendLine("<p class=\"NavigatorStyle\"><a class=\"NavigatorStyle\" href=\"#top\">Top</a></p>");
            }

            // footer
            builder.AppendLine("<p class=\"Normal\"></br></br></p>");
            builder.AppendLine("<p class=\"Footer\">Generated by SecureDelete. Copyright &copy 2008 <a href=\"mailto:lgratian@gmail.com\">Lup Gratian</a></p>");

            // end of document
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            return builder.ToString();
        }
    }
}
