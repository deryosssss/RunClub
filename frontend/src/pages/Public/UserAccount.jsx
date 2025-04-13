import React, { useEffect, useState } from "react";
import { useApp } from "../../context/AppContext";
import api from "../../services/api";
import "./UserAccountPage.css";

const DEFAULT_AVATAR =
  "https://raw.githubusercontent.com/johnmccants002/runclub-frontend/refs/heads/main/assets/images/profile.png";

const UserAccountPage = () => {
  const { user, logout } = useApp();
  const [form, setForm] = useState({
    name: "",
    location: "",
    age: "",
    photoUrl: DEFAULT_AVATAR,
  });

  const [editing, setEditing] = useState(false);
  const [saving, setSaving] = useState(false);
  const [previewImage, setPreviewImage] = useState(DEFAULT_AVATAR);

  useEffect(() => {
    if (user) {
      setForm({
        name: user.name || "",
        location: user.location || "",
        age: user.age || "",
        photoUrl: user.photoUrl || DEFAULT_AVATAR,
      });
      setPreviewImage(user.photoUrl || DEFAULT_AVATAR);
    }
  }, [user]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handlePhotoChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const imageUrl = URL.createObjectURL(file);
      setPreviewImage(imageUrl);
    }
  };

  const handleSave = async () => {
    setSaving(true);
    try {
      await api.put("/account/profile", {
        userId: user.userId,
        name: form.name,
        email: user.email,
        location: form.location,
        age: parseInt(form.age) || 0,
        photoUrl: previewImage || form.photoUrl,
      });
      alert("✅ Profile updated successfully!");
    } catch (err) {
      alert("❌ Failed to update profile");
    } finally {
      setSaving(false);
      setEditing(false);
    }
  };

  return (
    <div className="runclub-profile-container">
      <div className="runclub-card">
        <div className="avatar-section">
          <img src={previewImage} alt="Profile" className="profile-img" />
          {editing && (
            <>
              <label htmlFor="photoUpload" className="upload-btn">
                📸 Change Photo
              </label>
              <input
                type="file"
                id="photoUpload"
                name="photoUpload"
                accept="image/*"
                onChange={handlePhotoChange}
                style={{ display: "none" }}
              />
            </>
          )}
        </div>

        <h2>{form.name || "Runner Name"}</h2>
        <p className="role">{user?.role}</p>

        {user?.role?.toLowerCase() === 'runner' && (
          <div className="runclub-stats">
            <div>
              <h3>{user?.enrollmentsCount ?? 0}</h3>
              <p>Enrollments</p>
            </div>
            <div>
              <h3>{user?.completedCount ?? 0}</h3>
              <p>Completed</p>
            </div>
          </div>
        )}


        <div className="details-box">

          <p>
            <strong>Email:</strong> {user?.email || "Not set"}
          </p>
          <p>
            <strong>Location:</strong>{" "}
            {editing ? (
              <input
                name="location"
                value={form.location}
                onChange={handleChange}
              />
            ) : (
              form.location || "Not set"
            )}
          </p>
          <p>
            <strong>Age:</strong>{" "}
            {editing ? (
              <input
                name="age"
                type="number"
                value={form.age}
                onChange={handleChange}
              />
            ) : (
              form.age || "Not set"
            )}
          </p>
        </div>

        <div className="button-group">
          {editing ? (
            <button onClick={handleSave} disabled={saving}>
              {saving ? "Saving..." : "💾 Save Changes"}
            </button>
          ) : (
            <button onClick={() => setEditing(true)}>Edit Profile</button>
          )}
          <button className="secondary-btn"> Change Password</button>
          <button className="gray-btn" onClick={logout}>
            Logout
          </button>
          <button className="delete-btn">Delete My Account</button>
        </div>
      </div>
    </div>
  );
};

export default UserAccountPage;
