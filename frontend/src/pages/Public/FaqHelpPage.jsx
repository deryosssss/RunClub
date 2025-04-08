// src/pages/Public/FaqHelpPage.jsx
import React from 'react'

const FaqHelpPage = () => {
  return (
    <div className="container py-5">
      <h2 className="fw-bold text-center mb-4">❓ FAQ & Help Center</h2>

      <div className="mb-5">
        <h4>💬 Frequently Asked Questions</h4>
        <ul className="list-group list-group-flush">
          <li className="list-group-item">
            <strong>How do I register for an event?</strong>
            <p>Go to the Events page, select an event, and click “Enroll.” You must be logged in.</p>
          </li>
          <li className="list-group-item">
            <strong>Can I change my enrollment?</strong>
            <p>Yes. Go to the Enrollments tab in your dashboard to edit or remove entries.</p>
          </li>
          <li className="list-group-item">
            <strong>I forgot my password!</strong>
            <p>Use the "Forgot Password" link on the login page or contact us below.</p>
          </li>
        </ul>
      </div>

      <div>
        <h4>📬 Contact Us</h4>
        <form className="mt-3" onSubmit={e => { e.preventDefault(); alert('✅ Message sent!') }}>
          <div className="mb-3">
            <label className="form-label">Your Name</label>
            <input className="form-control" required />
          </div>
          <div className="mb-3">
            <label className="form-label">Email</label>
            <input type="email" className="form-control" required />
          </div>
          <div className="mb-3">
            <label className="form-label">Message</label>
            <textarea className="form-control" rows={4} required />
          </div>
          <button className="btn btn-primary">Send Message</button>
        </form>
      </div>
    </div>
  )
}

export default FaqHelpPage
