// src/pages/Public/FaqHelpPage.jsx

import React, { useState } from 'react'
import Header from '../../components/Header'
import { useApp } from '../../context/AppContext'

const initialFaqs = [
  {
    question: 'How do I register for an event?',
    answer: 'Go to the Events page, select an event, and click “Enroll.” You must be logged in.',
  },
  {
    question: 'Can I change my enrollment?',
    answer: 'Yes. Go to the Enrollments tab in your dashboard to edit or remove entries.',
  },
  {
    question: 'I forgot my password!',
    answer: 'Use the "Forgot Password" link on the login page or contact us below.',
  },
]

const FaqHelpPage = () => {
  const { user } = useApp()
  const isAdmin = user?.role?.toLowerCase() === 'admin'

  const [faqs, setFaqs] = useState(initialFaqs)
  const [editing, setEditing] = useState(false)

  const handleFaqChange = (index, field, value) => {
    const updated = [...faqs]
    updated[index][field] = value
    setFaqs(updated)
  }

  const handleAddFaq = () => {
    setFaqs(prev => [...prev, { question: '', answer: '' }])
  }

  const handleSave = () => {
    // Future: Send to backend here
    alert('✅ FAQs updated successfully!')
    setEditing(false)
  }

  return (
    <div>
      <Header />

      <div className="container py-5">
        <h2 className="fw-bold text-center mb-4">❓ FAQ & Help Center</h2>

        <div className="mb-5">
          <div className="d-flex justify-content-between align-items-center mb-2">
            <h4>💬 Frequently Asked Questions</h4>
            {isAdmin && (
              <div className="d-flex gap-2">
                <button
                  className={`btn btn-sm ${editing ? 'btn-secondary' : 'btn-warning'}`}
                  onClick={() => setEditing(!editing)}
                >
                  {editing ? 'Cancel Edit' : '✏️ Edit FAQs'}
                </button>
                {editing && (
                  <button className="btn btn-sm btn-success" onClick={handleAddFaq}>
                    ➕ Add FAQ
                  </button>
                )}
              </div>
            )}
          </div>

          <ul className="list-group list-group-flush">
            {faqs.map((faq, index) => (
              <li key={index} className="list-group-item">
                {editing ? (
                  <>
                    <input
                      className="form-control mb-2"
                      value={faq.question}
                      onChange={(e) => handleFaqChange(index, 'question', e.target.value)}
                      placeholder="Question"
                    />
                    <textarea
                      className="form-control"
                      value={faq.answer}
                      rows={2}
                      onChange={(e) => handleFaqChange(index, 'answer', e.target.value)}
                      placeholder="Answer"
                    />
                  </>
                ) : (
                  <>
                    <strong>{faq.question}</strong>
                    <p>{faq.answer}</p>
                  </>
                )}
              </li>
            ))}
          </ul>

          {editing && (
            <button className="btn btn-primary mt-3" onClick={handleSave}>
              💾 Save Changes
            </button>
          )}
        </div>

        <div>
          <h4>📬 Contact Us</h4>
          <form className="mt-3" onSubmit={(e) => { e.preventDefault(); alert('✅ Message sent!') }}>
            <div className="mb-3">
              <label htmlFor="name" className="form-label">Your Name</label>
              <input id="name" name="name" className="form-control" required />
            </div>
            <div className="mb-3">
              <label htmlFor="email" className="form-label">Email</label>
              <input id="email" name="email" type="email" className="form-control" required />
            </div>
            <div className="mb-3">
              <label htmlFor="message" className="form-label">Message</label>
              <textarea id="message" name="message" className="form-control" rows={4} required />
            </div>
            <button className="btn btn-primary">Send Message</button>
          </form>
        </div>
      </div>
    </div>
  )
}

export default FaqHelpPage
