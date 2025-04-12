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
      // Optional future: upload to cloud and save actual URL
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
      console.error(err);
      alert("❌ Failed to update profile");
    } finally {
      setSaving(false);
      setEditing(false);
    }
  };

  return (
    <div className="account-container">
      <h1 className="account-title">Account</h1>

      <div className="profile-section">
        <img src={previewImage} alt="Profile" className="profile-image" />

        {editing && (
          <div className="upload-photo">
            <label htmlFor="photoUpload" className="upload-button">
              📸 Upload New Photo
            </label>
            <input
              type="file"
              id="photoUpload"
              name="photoUpload"
              accept="image/*"
              onChange={handlePhotoChange}
              style={{ display: "none" }}
            />
          </div>
        )}

        <div className="profile-name">{form.name}</div>
      </div>

      <div className="details">
        <p><span className="detail-label"> Email:</span> {user?.email}</p>

        <p>
          <span className="detail-label"> Location:</span>{" "}
          {editing ? (
            <input
              type="text"
              name="location"
              value={form.location}
              onChange={handleChange}
              className="input-field"
            />
          ) : (
            <span className="location">{form.location || "Not set"}</span>
          )}
        </p>

        <p>
          <span className="detail-label"> Age:</span>{" "}
          {editing ? (
            <input
              type="number"
              name="age"
              value={form.age}
              onChange={handleChange}
              className="input-field"
            />
          ) : (
            <span>{form.age || "Not specified"}</span>
          )}
        </p>
        {user?.role?.toLowerCase() === 'runner' && (
          <>
            <p><span className="detail-label"> Enrollments:</span> {user?.enrollmentsCount}</p>
            <p><span className="detail-label"> Completed:</span> {user?.completedCount}</p>
          </>
        )}

      </div>

      <div className="actions">
        {editing ? (
          <button
            className="button save-button"
            onClick={handleSave}
            disabled={saving}
            style={{ opacity: saving ? 0.6 : 1 }}
          >
            {saving ? "⏳ Saving..." : " Save Changes"}
          </button>
        ) : (
          <button className="button edit-button" onClick={() => setEditing(true)}>
            ✏️ Edit Profile
          </button>
        )}

        <button className="button password-button"> Change Password</button>
        <button className="button logout-button" onClick={logout}>📤 Logout</button>
      </div>

      <div className="danger-zone">
        ⚠️ <span className="font-semibold">Danger Zone:</span> Delete Account ❌
        <div>
          <button className="mt-2 underline">Delete My Account</button>
        </div>
      </div>
    </div>
  );
};

export default UserAccountPage;
