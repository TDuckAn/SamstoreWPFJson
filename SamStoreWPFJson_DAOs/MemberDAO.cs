using SamStoreWPFJson_BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SamStoreWPFJson_DAOs
{
    public class MemberDAO
    {
        private readonly string _jsonFilePath;
        private List<Member> _members;

        public MemberDAO()
        {
            // Use the direct path to the JSON file in the project directory
            _jsonFilePath = @"H:\Class\P_R_N\SamStoreWPFJson-20250725T060713Z-1-001\SamStoreWPFJson\SamStoreWPFJson_JSONs\Members.json";
            LoadMembersFromJson();
        }

        private void LoadMembersFromJson()
        {
            try
            {
                if (File.Exists(_jsonFilePath))
                {
                    string json = File.ReadAllText(_jsonFilePath);
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        _members = new List<Member>();
                    }
                    else
                    {
                        _members = JsonSerializer.Deserialize<List<Member>>(json) ?? new List<Member>();
                    }
                }
                else
                {
                    _members = new List<Member>();
                }
                
                // Always save after loading/initializing to ensure file exists with proper content
                SaveMembersToJson();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading Members.json at {_jsonFilePath}: {ex.Message}", ex);
            }
        }

        private void SaveMembersToJson()
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                };
                string json = JsonSerializer.Serialize(_members, options);
                File.WriteAllText(_jsonFilePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving Members.json at {_jsonFilePath}: {ex.Message}", ex);
            }
        }

        public Member? GetMemberByLogin(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            try
            {
                // Reload members to ensure we have the latest data
                LoadMembersFromJson();
                
                if (_members == null || _members.Count == 0)
                {
                    throw new Exception($"No members found in JSON file at {_jsonFilePath}");
                }

                return _members.FirstOrDefault(m => 
                    m.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && 
                    m.Password.Equals(password, StringComparison.Ordinal));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error accessing members data: {ex.Message}", ex);
            }
        }
    }
}
