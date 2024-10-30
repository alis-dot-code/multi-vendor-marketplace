import React, { useCallback, useState } from 'react'

interface ImageUploaderProps {
  maxFiles?: number
  maxSizeMB?: number
  onChange?: (files: File[]) => void
}

export const ImageUploader: React.FC<ImageUploaderProps> = ({ maxFiles = 5, maxSizeMB = 5, onChange }) => {
  const [files, setFiles] = useState<File[]>([])

  const addFiles = useCallback(
    (incoming: FileList | null) => {
      if (!incoming) return
      const arr = Array.from(incoming)
      const valid = arr.filter((f) => f.size <= maxSizeMB * 1024 * 1024)
      const next = [...files, ...valid].slice(0, maxFiles)
      setFiles(next)
      onChange?.(next)
    },
    [files, maxFiles, maxSizeMB, onChange]
  )

  const remove = (index: number) => {
    const next = files.filter((_, i) => i !== index)
    setFiles(next)
    onChange?.(next)
  }

  return (
    <div>
      <label className="block w-full rounded-md border-dashed border-2 border-gray-200 p-6 text-center cursor-pointer">
        <input type="file" accept="image/*" multiple className="hidden" onChange={(e) => addFiles(e.target.files)} />
        <div className="text-sm text-gray-500">Drag & drop or click to upload images (max {maxFiles})</div>
      </label>
      <div className="mt-3 grid grid-cols-4 gap-2">
        {files.map((f, i) => (
          <div key={i} className="relative h-24 overflow-hidden rounded-md border">
            <img src={URL.createObjectURL(f)} className="object-cover h-full w-full" />
            <button type="button" onClick={() => remove(i)} className="absolute top-1 right-1 bg-white rounded-full p-1 text-sm">×</button>
          </div>
        ))}
      </div>
    </div>
  )
}

export default ImageUploader
