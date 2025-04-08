// src/pages/Public/MediaGalleryPage.jsx
import React from 'react'

const images = [
  'https://source.unsplash.com/600x400/?marathon,1',
  'https://source.unsplash.com/600x400/?marathon,2',
  'https://source.unsplash.com/600x400/?runners,3',
  'https://source.unsplash.com/600x400/?trail,4',
  'https://source.unsplash.com/600x400/?athlete,5',
  'https://source.unsplash.com/600x400/?fitness,6',
]

const MediaGalleryPage = () => {
  return (
    <div className="container py-5">
      <h2 className="fw-bold mb-4 text-center">🖼️ Media Gallery</h2>
      <p className="text-muted text-center mb-5">A collection of memories and milestones captured from our past events.</p>

      <div className="row g-4">
        {images.map((src, i) => (
          <div key={i} className="col-sm-6 col-md-4">
            <img
              src={src}
              alt={`Gallery ${i + 1}`}
              className="img-fluid rounded shadow-sm"
              style={{ cursor: 'pointer', transition: 'transform 0.2s ease-in-out' }}
              onMouseOver={e => (e.currentTarget.style.transform = 'scale(1.03)')}
              onMouseOut={e => (e.currentTarget.style.transform = 'scale(1)')}
            />
          </div>
        ))}
      </div>
    </div>
  )
}

export default MediaGalleryPage
